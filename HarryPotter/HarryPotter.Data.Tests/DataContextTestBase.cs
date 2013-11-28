using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImproveIT.Data;
using NDbUnit.Core;
using NUnit.Framework;
using System.Configuration;
using System.Data;
using HarryPotter.Domain;

namespace HarryPotter.Data.Tests
{
    public class DataContextTestsBase
    {

        private IDataContext _dataContext;
        private INDbUnitTest _ndbUnitTest;

        [SetUp]
        public void Setup()
        {
            string cnn = ConfigurationManager.ConnectionStrings["storedb_development"].ConnectionString;
            //this._ndbUnitTest = new NDbUnit.Core.MySqlClient.MySqlDbUnitTest(cnn);
            this._ndbUnitTest = new NDbUnit.Core.SqlClient.SqlDbUnitTest(cnn);
            this._ndbUnitTest.ReadXmlSchema("StoreSchema.xsd");
            this._ndbUnitTest.PerformDbOperation(NDbUnit.Core.DbOperationFlag.DeleteAll);

            NHibernate.ISession session =
                HarryPotter.Data.DataContextBuilder.BuildSession();
            this._dataContext = new ImproveIT.Data.Hibernate.HibernateDataContext(session);
        }
    }
}