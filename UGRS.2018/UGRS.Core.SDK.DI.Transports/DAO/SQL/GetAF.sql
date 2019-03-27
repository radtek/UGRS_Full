Select T0.PrcCode, T1.FrgnName, t1.ItemName from OPRC T0 with (nolock)
inner join OITM T1 with (nolock) on T0.PrcCode = t1.ItemCode
inner join (
   select a0.OcrCode, a0.ItemCode from ITM6 A0 
   inner join ( select max(linenum) Linenum, itemcode from ITM6 with (nolock) group by itemcode) A1 
   on a0.ItemCode = a1.ItemCode and a1.Linenum=a0.LineNum 
) T2 on T2.ItemCode = T0.PrcCode
where T0.DimCode = '2' and  T2.OcrCode = '{CostingCode}' 
AND T0.PrcCode = COALESCE(NULLIF('{PrcCode}',''),PrcCode) 
AND T1.U_GLO_Equipo = '{Equip}'
AND (T0.PrcCode like '%{Search}%' or T1.FrgnName like '%{Search}%' or t1.ItemName like '%{Search}%')