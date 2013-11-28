using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ImproveIT.Data;
using System.Data;
using HarryPotter.Domain;
using System.Configuration;
using NDbUnit.Core;

namespace HarryPotter.Data.Tests
{

    [TestFixture]
    public class DataContextTestsAuthors
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

        [Test]
        public void Add_EmptyDatabase_OneRowInserted()
        {
            // Arrange
            Author author = new Author() { Name = "J. K. Rowling" };

            //Act
            this._dataContext.Add<Author>(author);

            //Assert
            DataSet dataSet = this._ndbUnitTest.GetDataSetFromDb();
            Assert.AreEqual(1, dataSet.Tables["authors"].Rows.Count);
        }

        [Test]
        public void GetAll_DatabaseWith3Rows_Get3Rows()
        {
            // Arrange
            this._ndbUnitTest.ReadXml("data/authors.xml");
            this._ndbUnitTest.PerformDbOperation(NDbUnit.Core.DbOperationFlag.Insert);

            //Act
            IList<Author> authors = _dataContext.GetAll<Author>();

            //Assert
            Assert.AreEqual(3, authors.Count);
        }
    }
}