# Modified
## Filename
GetSalesSummaryOutput.cs
## Relative Path
inzibackend.Application.Shared\Tenants\Dashboard\Dto\GetSalesSummaryOutput.cs
## Language
C#
## Summary
The modified file defines a class GetSalesSummaryOutput with properties for sales metrics (TotalSales, Revenue, Expenses, Growth) and a collection of SalesSummaryData. It includes constructor changes to accept List<SalesSummaryData>.
## Changes
Added namespace using System.Collections.Generic;namespace inzibackend.Tenants.Dashboard.Dto{    public class GetSalesSummaryOutput    {        public GetSalesSummaryOutput(List<SalesSummaryData> salesSummary)        {            SalesSummary = salesSummary;        }        public int TotalSales { get; set; }        public int Revenue { get; set; }        public int Expenses { get; set; }        public int Growth { get; set; }        public List<SalesSummaryData> SalesSummary { get; set; }    }}
## Purpose
The file is part of an ASP.NET Zero application, defining a data model for sales summary output.
