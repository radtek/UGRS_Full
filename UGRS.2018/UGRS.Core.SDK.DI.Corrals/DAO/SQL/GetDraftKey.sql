
select top 1 * from ODRF t1
inner join DRF1 t2 on t1.DocEntry = t2.DocEntry
 where 
 t1.U_GLO_BusinessPartner = '{CardCode}'  
  and 
  U_GLO_Status = 'A' 
  and 
  t1.ObjType = 60
  order by t1.DocDate desc