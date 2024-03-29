﻿using Amazon.DynamoDBv2.DataModel;
using MTGLambda.MTGLambda.DataRepository;
using MTGLambda.MTGLambda.Helpers.DynamoDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services.Common
{
    public interface IService
    {
        void Init(ServiceContext serviceContext);
    }

    public abstract class BaseService : IService
    {
        protected ServiceContext SvcContext;

        public virtual void Init(ServiceContext serviceContext)
        {
            this.SvcContext = serviceContext;
        }
    }

    public class ServiceContext
    {
        public DaoFactory Repository { get; set; }
        public DynamoDBContext DynamoContext { get; set; }

        public ServiceContext()
        {
            Repository = new DaoFactory();
            DynamoContext = new DynamoDBContext(DynamoDbHelper.GetClient());
        }
    }
}
