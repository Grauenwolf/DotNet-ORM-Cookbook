﻿namespace Recipes.Ado.SingleModelCrud
{
    public class EmployeeClassification : Recipes.SingleModelCrud.IEmployeeClassification
    {
        public int EmployeeClassificationKey { get; set; }

        /// <exclude />
        public string? EmployeeClassificationName { get; set; }
    }
}
