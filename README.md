# Visma/e-conomic: Upload debitor invoices from .csv file

C# console application to upload invoices from .csv file to Visma/e-conomic debitor invoice gui, with manual approval.

Invoices is prepared in Excel and the sheet is saved as .csv file.
One invoice in one line, unlimited number of invoices.

This file is then imported by the application to Visma/e-conomic.
The invoices must manually be selected and persisted.

The authentication is saved in a local machine specific configuration file.

The Excel sheet/.csv file must comply to the following specification.

General:
First column must containg a tag. Lines NOT starting with a valid tag is ignored.
A tag starts with '#' followed by a name. The name is NOT case sencitive.

## Tags
|Tag       | Description       |
|----------|-------------------|
|#Tags     |Specifies usage of other columns|
|#Kundegrupper|Accepted customer groups (number from e-conomic)|
|#Text1|Headline on all following invoices|
|#Text2|Footer on all following invices|
|#Bilagsdato|Invoice date|
|#Betalingsbetingelse|Payment rule (number from e-conomic)|
|#Product|Item number in e-conoimic, apply to the came columns as specifid by #Tags|
|#Text|Item text on invoice, apply to the came columns as specifid by #Tags|
|#Enhedspris|Item sales price, apply to the came columns as specifid by #Tags|

Programming section:


Invoice section:
