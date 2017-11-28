﻿using System;
using System.Collections.Generic;
using System.Linq;

using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToQuery
    {
        private class Employees_ByName : AbstractIndexCreationTask<Employee>
        {
        }

        private interface IFoo
        {
            #region query_1_0
            IRavenQueryable<T> Query<T>(string indexName = null, string collectionName = null,
                                        bool isMapReduce = false);

            IRavenQueryable<T> Query<T, TIndexCreator>()
                where TIndexCreator : AbstractIndexCreationTask, new();
            #endregion
        }

        public HowToQuery()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_1_1
                    // load all entities from 'Employees' collection
                    List<Employee> employees = session
                        .Query<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_1_2
                    // load all entities from 'Employees' collection
                    // where FirstName equals 'Robert'
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_1_3
                    // load all entities from 'Employees' collection
                    // where FirstName equals 'Robert'
                    IRavenQueryable<Employee> employees = from employee in session.Query<Employee>()
                                 where employee.FirstName == "Robert"
                                 select employee;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_1_4
                    // load all entities from 'Employees' collection
                    // where FirstName equals 'Robert'
                    // using 'Employees/ByName' index
                    IRavenQueryable<Employee> employees = from employee in session.Query<Employee>("Employees/ByName")
                                 where employee.FirstName == "Robert"
                                 select employee;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_1_5
                    // load all entities from 'Employees' collection
                    // where FirstName equals 'Robert'
                    // using 'Employees/ByName' index
                    IRavenQueryable<Employee> employees = from employee in session.Query<Employee, Employees_ByName>()
                                 where employee.FirstName == "Robert"
                                 select employee;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_1_6
                    // load all employees hired between
                    // 1/1/2002 and 12/31/2002
                    List<Employee> employees = session
                        .Advanced.DocumentQuery<Employee>()
                        .WhereBetween(x => x.HiredAt, new DateTime(2002, 1, 1), new DateTime(2002, 12, 31))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_1_7
                    // load all entities from 'Employees' collection
                    // where FirstName equals 'Robert'
                    List<Employee> employees = session
                        .Advanced.RawQuery<Employee>("from Employees where FirstName = 'Robert'")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
