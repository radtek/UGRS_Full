declare @FolioOri nvarchar(10)
set @FolioOri= '{RiseId}'

select  
 w0.ItemCode, w0.Dscription ItemName, sum(isnull(w0.Quantity, 0)) Qty, sum(isnull(w0.Quantity, 0)) - max(isnull(w1.Quantity, 0)) - max(isnull(w3.Quantity, 0)) Quantity, 
 CASE   
    WHEN W2.QryGroup26 = 'Y' THEN '15W40'
	WHEN W2.QryGroup28 = 'Y' THEN 'Hidraulico'
	WHEN W2.QryGroup29 = 'Y' THEN 'SAE40'
	WHEN W2.QryGroup30 = 'Y' THEN 'Transmision'
	WHEN W2.QryGroup23 = 'Y' THEN 'Grasas'
	ELSE 'Unk'
 END Category
from
(select t0.DocEntry, t1.OcrCode2, 'A' Tipo, t1.ItemCode, t1.Dscription, t1.Quantity
from OWTR T0
inner join wtr1 t1 on t1.DocEntry = t0.DocEntry
where t1.WhsCode ='MQHEOBRA' and /*T0.U_GLO_DocEUG = @FolioOri*/ t1.OcrCode2 is not null and T0.U_MQ_Rise = @FolioOri /*IN (SELECT T2.U_IdRise AS Folio
																												FROM [@UG_TBL_MQ_RISE] T1 
																												INNER JOIN [@UG_TBL_MQ_RISE] T2 
																													ON T1.U_OriginalFolio = T2.U_OriginalFolio
																												WHERE T1.U_IdRise = @FolioOri
																												UNION ALL
																												SELECT T3.U_OriginalFolio AS Folio FROM [@UG_TBL_MQ_RISE] T3 WHERE T3.U_IdRise = @FolioOri) */
group by t0.DocEntry, t1.OcrCode2, t1.ItemCode, t1.Dscription, t1.Quantity
union all
select t0.DocEntry, t1.OcrCode2, 'C' Tipo, t1.ItemCode, t1.Dscription, t1.Quantity
from OPCH T0
inner join PCH1 t1 on t0.DocEntry = t1.DocEntry
where t1.WhsCode ='MQHEOBRA' and t0.CANCELED ='N' /*and t0.U_GLO_DocEUG = @FolioOri*/ and t1.OcrCode2 is not null and T0.U_MQ_Rise = @FolioOri /*IN (SELECT T2.U_IdRise AS Folio
																												FROM [@UG_TBL_MQ_RISE] T1 
																												INNER JOIN [@UG_TBL_MQ_RISE] T2 
																													ON T1.U_OriginalFolio = T2.U_OriginalFolio
																												WHERE T1.U_IdRise = @FolioOri
																												UNION ALL
																												SELECT T3.U_OriginalFolio AS Folio FROM [@UG_TBL_MQ_RISE] T3 WHERE T3.U_IdRise = @FolioOri) */
group by t0.DocEntry, t1.OcrCode2, t1.ItemCode, t1.Dscription, t1.Quantity
) W0

LEFT JOIN
(
SELECT T1.ItemCode, T1.Dscription, sum(isnull(T1.Quantity, 0)) Quantity FROM OIGE T0
INNER JOIN IGE1 T1 ON T0.DocEntry = T1.DocEntry
WHERE /*T0.U_GLO_DocEUG = @FolioOri AND*/ T0.CANCELED = 'N' and T0.U_MQ_Rise = @FolioOri /*IN (SELECT T2.U_IdRise AS Folio
																												FROM [@UG_TBL_MQ_RISE] T1 
																												INNER JOIN [@UG_TBL_MQ_RISE] T2 
																													ON T1.U_OriginalFolio = T2.U_OriginalFolio
																												WHERE T1.U_IdRise = @FolioOri
																												UNION ALL
																												SELECT T3.U_OriginalFolio AS Folio FROM [@UG_TBL_MQ_RISE] T3 WHERE T3.U_IdRise = @FolioOri)*/ 
GROUP BY t1.ItemCode, t1.Dscription
) W1 ON W1.ItemCode = W0.ItemCode
LEFT JOIN
(
SELECT T1.ItemCode, T1.Dscription, sum(isnull(T1.Quantity, 0)) Quantity FROM OWTR T0
INNER JOIN WTR1 T1 ON T0.DocEntry = T1.DocEntry
WHERE /*T0.U_GLO_DocEUG = @FolioOri AND*/ T0.CANCELED = 'N' and t1.FromWhsCod='MQHEOBRA' and T0.U_MQ_Rise = @FolioOri /*IN (SELECT T2.U_IdRise AS Folio
																												FROM [@UG_TBL_MQ_RISE] T1 
																												INNER JOIN [@UG_TBL_MQ_RISE] T2 
																													ON T1.U_OriginalFolio = T2.U_OriginalFolio
																												WHERE T1.U_IdRise = @FolioOri
																												UNION ALL
																												SELECT T3.U_OriginalFolio AS Folio FROM [@UG_TBL_MQ_RISE] T3 WHERE T3.U_IdRise = @FolioOri) */
GROUP BY t1.ItemCode, t1.Dscription
) W3 ON W3.ItemCode = W0.ItemCode
INNER JOIN OITM W2 ON W2.ItemCode = W0.ItemCode
group by w0.ItemCode, w0.Dscription,w1.Quantity, W2.QryGroup26, W2.QryGroup28, W2.QryGroup29, W2.QryGroup30, W2.QryGroup23