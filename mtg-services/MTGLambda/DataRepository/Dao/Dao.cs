using Amazon.DynamoDBv2.DocumentModel;
using MTGLambda.MTGLambda.DataRepository.Dto;
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
            return FindAll(filter, string.Empty, 0);
        }

        public IEnumerable<T> FindAll(string filter, string orderByExpression, int recordCap)
        {
            var request = new LoadTableItemsRequest
            {
                Filter = filter,
                OrderByExpression = orderByExpression,
                RecordCap = recordCap,
                Table = typeof(T).Name, //Name of table will be name of model
                Properties = typeof(T).GetProperties().Select(x => x.Name).ToList()
            };

            var response = _daoContext.LoadTableItems(request);

            if (response.IsSuccess)
            {
                foreach(var tableItem in response.TableItems)
                {
                    yield return ContextResponseEntity(tableItem);
                }
            }
        }

        //TODO: Save/Delete

        private static T ContextResponseEntity(Document tableItem)
        {
            T t = (T)Activator.CreateInstance(typeof(T));

            foreach(PropertyInfo property in typeof(T).GetProperties())
            {
                var tableItemProperty = tableItem[property.Name];

                if (!(tableItemProperty == null))
                {
                    typeof(T).GetProperty(property.Name).SetValue(t, tableItemProperty.AsString());
                }
            }

            return t;
        }
    }
}
