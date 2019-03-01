SELECT T1.ItemCode, T1.Price FROM (SELECT ItemName, ItemCode FROM OITM) T0
INNER JOIN (SELECT PriceList, ItemCode, Price FROM ITM1) T1 ON T1.ItemCode = T0.ItemCode
INNER JOIN (SELECT ListNum, U_GLO_Location FROM OPLN) T2 ON T1.PriceList = T2.ListNum 
WHERE T0.ItemCode =  (select U_Value from [@UG_CONFIG] where Name='{ServiceName}') AND T2.U_GLO_Location = '{WhsCode}' --AND T2.U_GLO_WhsCode = '{WhsCode}'
