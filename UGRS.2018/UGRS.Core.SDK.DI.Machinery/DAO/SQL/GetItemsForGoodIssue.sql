declare @FolioOri nvarchar(10)
set @FolioOri= '{RiseId}'

select  
 w0.ItemCode, w2.ItemName ItemName, (isnull(w0.Quantity, 0)) Qty, (isnull(w0.Quantity, 0)) - isnull(w1.Quantity, 0) Quantity, 
 CASE   
    WHEN W2.QryGroup26 = 'Y' THEN '15W40'
       WHEN W2.QryGroup28 = 'Y' THEN 'Hidraulico'
       WHEN W2.QryGroup29 = 'Y' THEN 'SAE40'
       WHEN W2.QryGroup30 = 'Y' THEN 'Transmision'
       WHEN W2.QryGroup23 = 'Y' THEN 'Grasas'
       ELSE 'Unk'
END Category
from
(
select A0.ItemCode, sum(A0.Quantity) Quantity
from (
select  t1.ItemCode, sum(t1.Quantity) Quantity
from OWTR T0
inner join wtr1 t1 on t1.DocEntry = t0.DocEntry
where t1.WhsCode ='MQHEOBRA' and t1.Quantity>0 and t0.CANCELED='N' and
      T0.U_MQ_Rise IN ( SELECT T3.U_IdRise AS Folio FROM [@UG_TBL_MQ_RISE] T3 WHERE T3.U_OriginalFolio = @FolioOri or T3.U_IdRise = @FolioOri 
                             ) 
group by t1.ItemCode
union all
select  t1.ItemCode, sum(t1.Quantity) Quantity
from OPCH T0
inner join PCH1 t1 on t0.DocEntry = t1.DocEntry
where t1.WhsCode ='MQHEOBRA' and t0.CANCELED ='N' and
      T0.U_MQ_Rise IN ( SELECT T3.U_IdRise AS Folio FROM [@UG_TBL_MQ_RISE] T3 WHERE T3.U_OriginalFolio = @FolioOri or T3.U_IdRise = @FolioOri
                                                ) 
group by t1.ItemCode) A0 group by A0.ItemCode
) W0
LEFT JOIN
(
select a0.ItemCode, sum(a0.Quantity) Quantity 
from (
SELECT T1.ItemCode, sum(isnull(T1.Quantity, 0)) Quantity FROM OIGE T0
INNER JOIN IGE1 T1 ON T0.DocEntry = T1.DocEntry
WHERE T0.CANCELED = 'N' and T0.U_MQ_Rise IN ( SELECT T3.U_IdRise AS Folio FROM [@UG_TBL_MQ_RISE] T3 WHERE T3.U_OriginalFolio = @FolioOri or T3.U_IdRise = @FolioOri)
group by T1.ItemCode
union all
select  t1.ItemCode, sum(t1.Quantity) Quantity
from OWTR T0
inner join wtr1 t1 on t1.DocEntry = t0.DocEntry
where t1.FromWhsCod ='MQHEOBRA' and t1.Quantity>0 and t0.CANCELED='N' and
      T0.U_MQ_Rise IN (SELECT T3.U_IdRise AS Folio FROM [@UG_TBL_MQ_RISE] T3 WHERE T3.U_OriginalFolio = @FolioOri or T3.U_IdRise = @FolioOri)                                      
group by t1.ItemCode
) A0
group by a0.ItemCode       
) W1 ON W1.ItemCode = W0.ItemCode
INNER JOIN OITM W2 ON W2.ItemCode = W0.ItemCode