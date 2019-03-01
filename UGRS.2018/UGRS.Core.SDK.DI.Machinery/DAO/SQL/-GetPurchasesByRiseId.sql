SELECT T2.U_GLO_DocEUG AS "IdSubida", T2.ObjType, T3.OcrCode2 AS "ActivoCode", T5.PrcName AS "ActivoName", 
sum(
case when T4.QryGroup31 = 'Y' then isnull(T3.Quantity,0) else 0 end
) as "15w40",
sum(
case when T4.QryGroup33 = 'Y' then isnull(T3.Quantity,0) else 0 end
) as "Hidraulico",
sum(
case when T4.QryGroup34 = 'Y' then isnull(T3.Quantity,0) else 0 end
) as "SAE40",
sum(
case when T4.QryGroup35 = 'Y' then isnull(T3.Quantity,0) else 0 end
) as "Trasmision",
sum(
case when T4.QryGroup28 = 'Y' then isnull(T3.Quantity,0) else 0 end
) as "Grasas", 
sum(
case when T4.QryGroup26 = 'Y' then isnull(T3.Quantity,0) else 0 end
) as "Gasolina" 
FROM OPCH T2
INNER JOIN PCH1 T3
	ON T2.DocEntry = T3.DocEntry
INNER JOIN OITM T4
	ON T3.ItemCode = T4.ItemCode
INNER JOIN OPRC T5
	ON T3.OcrCode2 = T5.PrcCode
WHERE T2.U_GLO_DocEUG = {RiseId} 
GROUP BY T3.OcrCode2, T5.PrcName, T2.U_GLO_DocEUG, T2.ObjType 