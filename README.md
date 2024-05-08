# e-conomic: Upload debitor invoices from .csv file

C# console application to upload invoices from .csv file to Visma/e-conomic debitor invoice gui, with manual approval.

Invoices is prepared in Excel and the sheet is saved as .csv file.
One invoice in one line, unlimited number of invoices.

This file is then imported by the application to Visma/e-conomic.
The invoices must manually be selected and persisted.

The authentication against e-conomic is saved in a machine specific configuration file.

The Excel sheet/.csv file must comply to the following specification.

## General

First column must containg a tag. Lines NOT starting with a valid tag is ignored.
A tag starts with '#' followed by a name. The name is NOT case sencitive.

## Configuration Tags

|Tag       | Description       |Alternative tags|
|----------|-------------------|---------------|
|#customergroup|Accepted customer groups (number from e-conomic)|#customergroups, #kundegruppe, #kundegrupper|
|#text1|Headline on all following invoices|#tekst1|
|#text2|Footer on all following invices|#tekst2|
|#invoicedate|Invoice date|#bilagsdato|
|#paymentterm|Payment rule (number from e-conomic)|#Betalingsbetingelse|

## Column Configuration Tags

Values must appear in the columns specifid by the Column Configuration tag **#tags-#product**

|Tag       | Description       |Alternative tags|
|----------|-------------------|---------------|
|#tags     |Specifies usage of columns</br>**#debitornumber:** must appear once, </br>**#product:** must appear one or more times</br>Other columns will be ignored|</br>#debitornummer</br>#produkt|
|#product|Item number in e-conoimic, must be applied to all **#product** columns|#products, #produkt, #produkter|
|#text|Item text on invoice, must be applied to all **#product** columns|#tekst|
|#unittext|Product unit text (m2/m3/mdr/år/kW/kWh/...)|#enhedstekst|
|#unitnumber|Product id (number from e-conomic)|#enhedsnummer|
|#unitprice|Item sales price, apply to the came columns as specifid by **#Tags**|#Enhedspris|

## Invoice tag

Values must appear in the columns specifid by the Configuration tags #tags-#debitornumer or **#tags-#product**

|Tag     | Description       |Alternative tags|
|--------|-------------------|----------------|
|#invoice|Line with invoice data, </br>**#debitornumber column:** A debitor number from e-conomic must be provided once</br>**#product columns:** A value (decimal) expressing then number of units sold of this product, zero or blank for ignoring this product|#faktura|


### Usage:
>Eu.Iamia.Invoicing.ConsoleApp [options]

### Options:
>--Application:CsvFileFullName &lt;full path filename&gt;
>--Reporting:DiscardNonErrorLogfiles <true|false>    Only keep logfiles showing errors
>--version                                           Show version information
>-?, -h, --help                                      Show help and usage information