﻿using Recipes.PartialUpdate;
using System;
using Tortuga.Chain;

namespace Recipes.Chain.PartialUpdate
{
    public class PartialUpdateRepository : IPartialUpdateRepository<EmployeeClassification>
    {
        const string TableName = "HR.EmployeeClassification";
        readonly SqlServerDataSource m_DataSource;

        public PartialUpdateRepository(SqlServerDataSource dataSource)
        {
            m_DataSource = dataSource;
        }

        public int Create(EmployeeClassification classification)
        {
            if (classification == null)
                throw new ArgumentNullException(nameof(classification), $"{nameof(classification)} is null.");

            return m_DataSource.Insert(classification).ToInt32().Execute();
        }

        public EmployeeClassification? GetByKey(int employeeClassificationKey)
        {
            return m_DataSource.GetByKey(TableName, employeeClassificationKey).ToObject<EmployeeClassification>().NeverNull().Execute();
        }

        public void Update(EmployeeClassificationNameUpdater updateMessage)
        {
            if (updateMessage == null)
                throw new ArgumentNullException(nameof(updateMessage), $"{nameof(updateMessage)} is null.");

            m_DataSource.Update(TableName, updateMessage).Execute();
        }

        public void Update(EmployeeClassificationFlagsUpdater updateMessage)
        {
            if (updateMessage == null)
                throw new ArgumentNullException(nameof(updateMessage), $"{nameof(updateMessage)} is null.");

            m_DataSource.Update(TableName, updateMessage).Execute();
        }

        public void UpdateFlags(int employeeClassificationKey, bool isExempt, bool isEmployee)
        {
            m_DataSource.Update(TableName, new { employeeClassificationKey, isExempt, isEmployee }).Execute();
        }
    }
}
