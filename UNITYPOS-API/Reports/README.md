# Reports Folder

Use this folder for optional report-specific assets if you want to keep
JSON, HTML, or documentation files organized by report category.

Examples:

- `Reports/Sales/DailySalesReport.html`
- `Reports/Sales/BillWiseSalesReport.html`
- `Reports/Inventory/CurrentStockReport.json`

The current free reporting flow in this repo does not require a visual
designer file. Reports are driven by:

- `ReportCategory`
- `ReportMaster`
- `ReportFilters`
- `ReportPermission`
- stored procedures

`ReportMaster.TemplateName` and `TemplatePath` can still be used as optional
layout asset metadata if you later want to associate a JSON or HTML layout
file with a report.
