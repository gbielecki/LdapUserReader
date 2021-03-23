using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using System.Linq;
using System.DirectoryServices.AccountManagement;

namespace LdapUserReader
{
    class Program
    {
        private static string _basicUserGroup = "A-MLTApp-Uzytkownik";
        private static List<string> _basicUserGroups = new List<string>() { _basicUserGroup };

        static void Main(string[] args)
        {
            var connectionString = "";
            IDbConnection DbConnection; try
            {
                DbConnection = new SqlConnection(connectionString);
                DbConnection?.Open();
            }
            catch (Exception e)
            {
                Log.Information("Error initializing connections");
                var message = e.InnerException == null ? e.Message : e.InnerException.Message;
                throw;
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/LdapUserReader.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Ldap checker start!");

            int skippedCarriers = 0;
            List<Carrier> carriers;
            try
            {
                do
                {
                    carriers = GetAeosCarriers(DbConnection, 500, skippedCarriers);
                    foreach (var carrier in carriers)
                    {
                        var groups = GetGroups(carrier.PersonnelNr);
                        Log.Information($"personnelNr: {carrier.PersonnelNr}, email: {carrier.Email}, groups: {string.Join(", ", groups)}");
                    }
                    skippedCarriers += 500;
                } while (carriers.Count > 0);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IEnumerable<string> GetGroups(string userName, string domainName = null)
        {
            if (userName.Contains('\\') || userName.Contains('/'))
            {
                domainName = userName.Split(new char[] { '\\', '/' })[0];
                userName = userName.Split(new char[] { '\\', '/' })[1];
            }

            try
            {
                // establish domain context
                PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain, domainName);

                // find your user
                UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, userName);

                if (user == null)
                {
                    return _basicUserGroups;
                }
                var groups = user.GetAuthorizationGroups();
                if (!groups.Any())
                {
                    return _basicUserGroups;
                }
                var names = groups.Select(x => x.Name).ToList();

                return names.Where(x => x.StartsWith("A-MLTApp-"));
            }
            catch (PrincipalServerDownException ex)
            {
                Log.Error(ex, $"[PrincipalServerDownException] Error in LDAP when execute GetGroups:" + ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[Exception] Error when execute GetGroups():" + ex.Message);
            }
            return _basicUserGroups;
        }

        private static List<Carrier> GetAeosCarriers(IDbConnection dbConnection, int carriersToTake, int skippedCarriers)
        {
            try
            {
                return dbConnection.Query<Carrier>(
                    @"SELECT * FROM [carrier] WHERE [carriertype] = 1 and [removalstatus] = 0 
                    ORDER BY [objectid]
                    OFFSET (@skippedCarriers) ROWS FETCH NEXT (@carriersToTake) ROWS ONLY"
                    , new { carriersToTake, skippedCarriers }).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
