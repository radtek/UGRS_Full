--Activos fijos
Select T0.PrcCode, T1.FrgnName, t2.OcrCode from OPRC T0 with (Nolock)
inner join OITM T1 with (Nolock) on T0.PrcCode = t1.ItemCode
inner join (
   select a0.OcrCode, a0.ItemCode from ITM6 A0 with (Nolock)
   inner join ( select max(linenum) Linenum, itemcode from ITM6 with (Nolock) group by itemcode) A1 
   on a0.ItemCode = a1.ItemCode and a1.Linenum=a0.LineNum 
) T2 on T2.ItemCode = T0.PrcCode
where T0.DimCode = '2' --and  T2.OcrCode = '{OcrCode}'--and T2.PrcCode = '{Area}' 
