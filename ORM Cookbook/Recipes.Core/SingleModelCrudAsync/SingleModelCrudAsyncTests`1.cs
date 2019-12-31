﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.SingleModelCrudAsync
{
    /// <summary>
    /// This use case performs basic CRUD operations on a simple model without children.
    /// </summary>
    /// <typeparam name="TModel">A EmployeeClassification model or entity</typeparam>
    public abstract class SingleModelCrudAsyncTests<TModel> : TestBase
        where TModel : class, IEmployeeClassification, new()
    {
        protected abstract ISingleModelCrudAsyncRepository<TModel> GetRepository();

        /// <summary>
        /// Create and read back a row.
        /// </summary>
        [TestMethod]
        public async Task CreateAndReadBackAsync()
        {
            var repository = GetRepository();

            var newRecord = new TModel();
            newRecord.EmployeeClassificationName = "Test " + DateTime.Now.Ticks;
            var newKey = await repository.CreateAsync(newRecord).ConfigureAwait(false);
            Assert.IsTrue(newKey >= 1000, "keys under 1000 were not generated by the database");

            var echo = await repository.GetByKeyAsync(newKey).ConfigureAwait(false);
            Assert.IsNotNull(echo);
            Assert.AreEqual(newKey, echo!.EmployeeClassificationKey);
            Assert.AreEqual(newRecord.EmployeeClassificationName, echo.EmployeeClassificationName);

            var search = await repository.FindByNameAsync(newRecord.EmployeeClassificationName).ConfigureAwait(false);
            Assert.IsNotNull(search);
            Assert.AreEqual(newKey, search!.EmployeeClassificationKey);
            Assert.AreEqual(newRecord.EmployeeClassificationName, search.EmployeeClassificationName);
        }

        /// <summary>
        /// Create and delete a row.
        /// </summary>
        [TestMethod]
        public async Task CreateAndDeleteByModelAsync()
        {
            var repository = GetRepository();

            var newRecord = new TModel();
            newRecord.EmployeeClassificationName = "Test " + DateTime.Now.Ticks;
            var newKey = await repository.CreateAsync(newRecord).ConfigureAwait(false);
            Assert.IsTrue(newKey >= 1000, "keys under 1000 were not generated by the database");

            var echo = await repository.GetByKeyAsync(newKey).ConfigureAwait(false);

            Assert.IsNotNull(echo);
            await repository.DeleteAsync(echo!).ConfigureAwait(false);

            var all = await repository.GetAllAsync().ConfigureAwait(false);
            Assert.IsFalse(all.Any(ec => ec.EmployeeClassificationKey == newKey));
        }

        /// <summary>
        /// Create and delete a row.
        /// </summary>
        [TestMethod]
        public async Task CreateAndDeleteByKeyAsync()
        {
            var repository = GetRepository();

            var newRecord = new TModel();
            newRecord.EmployeeClassificationName = "Test " + DateTime.Now.Ticks;
            var newKey = await repository.CreateAsync(newRecord).ConfigureAwait(false);
            Assert.IsTrue(newKey >= 1000, "keys under 1000 were not generated by the database");

            await repository.DeleteByKeyAsync(newKey).ConfigureAwait(false);

            var all = await repository.GetAllAsync().ConfigureAwait(false);
            Assert.IsFalse(all.Any(ec => ec.EmployeeClassificationKey == newKey));
        }

        /// <summary>
        /// Get all rows from a table.
        /// </summary>
        [TestMethod]
        public async Task GetAllAsync()
        {
            var repository = GetRepository();

            var all = await repository.GetAllAsync().ConfigureAwait(false);
            Assert.IsNotNull(all);
            Assert.AreNotEqual(0, all.Count);
        }

        /// <summary>
        /// Get a row using a primary key.
        /// </summary>
        [TestMethod]
        public async Task GetByKeyAsync()
        {
            var repository = GetRepository();

            var ec1 = await repository.GetByKeyAsync(1).ConfigureAwait(false);
            var ec2 = await repository.GetByKeyAsync(2).ConfigureAwait(false);
            var ec3 = await repository.GetByKeyAsync(3).ConfigureAwait(false);

            Assert.IsNotNull(ec1);
            Assert.IsNotNull(ec2);
            Assert.IsNotNull(ec3);

            Assert.AreEqual(1, ec1!.EmployeeClassificationKey);
            Assert.AreEqual(2, ec2!.EmployeeClassificationKey);
            Assert.AreEqual(3, ec3!.EmployeeClassificationKey);
        }

        /// <summary>
        /// Create and update a row.
        /// </summary>
        [TestMethod]
        public async Task CreateAndUpdateAsync()
        {
            var repository = GetRepository();

            var newRecord = new TModel();
            newRecord.EmployeeClassificationName = "Test " + DateTime.Now.Ticks;
            var newKey = await repository.CreateAsync(newRecord).ConfigureAwait(false);
            Assert.IsTrue(newKey >= 1000); //keys under 1000 were not generated by the database

            var echo = await repository.GetByKeyAsync(newKey).ConfigureAwait(false);

            Assert.IsNotNull(echo);
            Assert.AreEqual(newRecord.EmployeeClassificationName, echo!.EmployeeClassificationName);
            echo.EmployeeClassificationName = "Updated " + DateTime.Now.Ticks;
            await repository.UpdateAsync(echo).ConfigureAwait(false);

            var updated = await repository.GetByKeyAsync(newKey).ConfigureAwait(false);
            Assert.IsNotNull(updated);
            Assert.AreEqual(echo.EmployeeClassificationName, updated!.EmployeeClassificationName);
        }

        [TestMethod]
        public async Task CreateAsync_ParameterCheck()
        {
            var repository = GetRepository();
            await AssertThrowsExceptionAsync<ArgumentNullException>(() => repository.CreateAsync(null!)).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task UpdateAsync_ParameterCheck()
        {
            var repository = GetRepository();
            await AssertThrowsExceptionAsync<ArgumentNullException>(() => repository.UpdateAsync(null!)).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task DeleteAsync_ParameterCheck()
        {
            var repository = GetRepository();
            await AssertThrowsExceptionAsync<ArgumentNullException>(() => repository.DeleteAsync(null!)).ConfigureAwait(false);
        }
    }
}
