using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using MTGLambda.MTGLambda.DataRepository.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository.Dao
{
    public abstract class Dao<T>
    {
        protected DaoContext _daoContext = null;

        public Dao(DaoContext daoContext)
        {
            _daoContext = daoContext;
        }

        public T Find(string id)
        {
            return FindAll($"id = {id}").SingleOrDefault();
        }

        public IEnumerable<T> FindAll()
        {
            return FindAll(string.Empty);
        }

        public IEnumerable<T> FindAll(string filter)
        {
            return FindAll(filter, new List<ScanCondition>(), string.Empty, 0);
        }

        public IEnumerable<T> FindAll(List<ScanCondition> conditions)
        {
            return FindAll(string.Empty, conditions, string.Empty, 0);
        }

        public IEnumerable<T> FindAll(string filter, List<ScanCondition> conditions, string orderByExpression, int recordCap)
        {
            LambdaLogger.Log($"Entering: FindAll({JsonConvert.SerializeObject(conditions)})");

            var request = new LoadTableItemsRequest
            {
                Filter = filter,
                OrderByExpression = orderByExpression,
                RecordCap = recordCap,
                Table = typeof(T).Name, //Name of table will be name of model
                Properties = typeof(T).GetProperties().Select(x => x.Name).ToList(),
                Type = typeof(T),
                Conditions = conditions
            };

            var response = _daoContext.LoadTableItems<T>(request);

            LambdaLogger.Log($"Load Table Items response: { JsonConvert.SerializeObject(response) }");

            return response;

            //if (response != null)
            //{
            //    foreach(var tableItem in response.TableItems)
            //    {
            //        yield return ContextResponseEntity(tableItem);
            //    }
            //
        }

        //TODO: Save/Delete
        public void Save(IEnumerable<T> saveItems)
        {
            List<T> updatedItems = new List<T>();

            try
            {
                List<T> records = saveItems.ToList();

                _daoContext.SaveTableItems<T>(saveItems);
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }

        private static Document ContextRequestEntity(T t)
        {
            return Document.FromJson(JsonConvert.SerializeObject(t));
        }

        private static T ContextResponseEntity(Document tableItem)
        {
            T t = (T)Activator.CreateInstance(typeof(T));

            foreach(PropertyInfo property in typeof(T).GetProperties())
            {
                var tableItemProperty = tableItem[property.Name];

                if (!(tableItemProperty == null))
                {
                    if (tableItemProperty is Primitive)
                        typeof(T).GetProperty(property.Name).SetValue(t, tableItemProperty.AsPrimitive());
                    else if (tableItemProperty is PrimitiveList)
                        typeof(T).GetProperty(property.Name).SetValue(t, tableItemProperty.AsListOfPrimitive());
                }
            }

            return t;
        }
    }
}
