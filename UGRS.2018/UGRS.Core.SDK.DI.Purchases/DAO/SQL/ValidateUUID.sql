--Validate UUID in OINV
select EDocNum, U_UDF_UUID from OPCH with (Nolock) where (EDocNum = '{UUID}' or U_UDF_UUID = '{UUID}') AND CANCELED = 'N'
