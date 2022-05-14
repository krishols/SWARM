using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using System.Data.OracleClient;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Reflection;
using SWARM.EF;
using SWARM.EF.Models;
using Microsoft.Extensions.Configuration;
using SWARM.EF.Data;
using SWARM.Shared.DTO;

namespace SWARM.UT.UT
{
    [TestClass]
    public class UT_Course
    {
        static IConfiguration Configuration = InitConfiguration();
        static DbContextOptionsBuilder<SWARMOracleContext> _optionsBuilder = new DbContextOptionsBuilder<SWARMOracleContext>();
        static SWARMOracleContext _context;

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }
        [ClassInitialize()]
        public static void InitTestSuite(TestContext testContext)
        {
            _optionsBuilder.UseOracle(Configuration.GetConnectionString("SwarmOracleConnection"));
            _context = new SWARMOracleContext(_optionsBuilder.Options);
        }

        /*
        [TestMethod]
        public void UpdateCost()
        {
            ICollection<Course> courses = _context.Courses.ToList();
            foreach (var crse in courses)
            {
                crse.Cost += 1000;
                _context.Update(crse);
            }
            _context.SaveChanges();
            Assert.IsTrue(1 == 1);
        }
        */
    }
}
