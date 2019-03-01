declare @FolioOri nvarchar(10)
set @FolioOri= '{RiseId}'

select  
 w0.OcrCode2 Equipo, max(w5.FrgnName) NumEconomico,
case when w0.Tipo ='A' then 'Traslado' else 'Compra' end Tipo, w6.U_TypeEQ EqType, 
sum(case when w4.QryGroup21='Y' then case when w2.DocEntry is not null then w2.Quantity else w3.Quantity end else 0 end) Gasolina,
sum(case when w4.QryGroup26='Y' then case when w2.DocEntry is not null then w2.Quantity else w3.Quantity end else 0 end) '15W40',
sum(case when w4.QryGroup28='Y' then case when w2.DocEntry is not null then w2.Quantity else w3.Quantity end else 0 end) Hidra,
sum(case when w4.QryGroup29='Y' then case when w2.DocEntry is not null then w2.Quantity else w3.Quantity end else 0 end) SAE,
sum(case when w4.QryGroup30='Y' then case when w2.DocEntry is not null then w2.Quantity else w3.Quantity end else 0 end) TRANS,
sum(case when w4.QryGroup23='Y' then case when w2.DocEntry is not null then w2.Quantity else w3.Quantity end else 0 end) Grasa,
sum(case when w4.QryGroup25='Y' then case when w2.DocEntry is not null then w2.Quantity else w3.Quantity end else 0 end) DieselT
from
(select t0.DocEntry, t1.OcrCode2, 'A' Tipo
from OWTR T0
inner join wtr1 t1 on t1.DocEntry = t0.DocEntry
where t1.WhsCode ='MQHEOBRA' and T0.U_MQ_Rise = @FolioOri and t1.OcrCode2 is not null and t1.Quantity>0 and t0.CANCELED='N'
group by t0.DocEntry, t1.OcrCode2
union all
select t0.DocEntry, t1.OcrCode2, 'C' Tipo  
from OPCH T0
inner join PCH1 t1 on t0.DocEntry = t1.DocEntry
where t1.WhsCode ='MQHEOBRA' and t0.CANCELED ='N' and t0.U_MQ_Rise=@FolioOri and t1.OcrCode2 is not null
group by t0.DocEntry, t1.OcrCode2
) W0
left join
(select T2.U_PrcCode 
 from [@UG_TBL_MQ_RISE] T0 with (nolock) 
 inner join [@UG_TBL_MQ_RISE] T1 with (nolock) on t0.U_DocRef = t1.U_IdRise 
 inner join [@UG_MQ_RIFR] T2 with (nolock) on t1.U_IdRise= t2.U_IdRise where T0.U_IdRise =@FolioOri
) WW on Ww.U_PrcCode = w0.OcrCode2
left join 
(
select 
B0.DocEntry, B0.OcrCode2, B0.Tipo
from (
select a1.DocDate, A1.DocEntry, a0.Tipo, A0.OcrCode2 from
(select min(t0.DocEntry) Folio, t1.OcrCode2, 'A' Tipo
from OWTR T0
inner join wtr1 t1 on t1.DocEntry = t0.DocEntry
where t1.WhsCode ='MQHEOBRA' and T0.U_MQ_Rise = @FolioOri and t1.OcrCode2 is not null and t1.Quantity>0 and t0.CANCELED='N'
group by t1.OcrCode2) A0
inner join OWTR A1 on a1.DocEntry = A0.Folio
union all
select 
a1.DocDate, A1.DocEntry, a0.Tipo , A0.OcrCode2
from 
(select min(t0.DocEntry) Folio, 'C' Tipo , t1.OcrCode2 
from OPCH T0
inner join PCH1 t1 on t0.DocEntry = t1.DocEntry
where t1.WhsCode ='MQHEOBRA' and t0.CANCELED ='N' and t0.U_MQ_Rise=@FolioOri and t1.OcrCode2 is not null
group by t1.OcrCode2) A0
inner join OPCH A1 on a0.Folio = a1.DocEntry
) B0
left join
(
select 
a1.DocDate, A1.DocEntry, a0.Tipo , A0.OcrCode2
from 
(select min(t0.DocEntry) Folio, 'C' Tipo , t1.OcrCode2 
from OPCH T0
inner join PCH1 t1 on t0.DocEntry = t1.DocEntry
where t1.WhsCode ='MQHEOBRA' and t0.CANCELED ='N' and t0.U_MQ_Rise=@FolioOri and t1.OcrCode2 is not null
group by t1.OcrCode2) A0
inner join OPCH A1 on a0.Folio = a1.DocEntry) B1 on  b1.OcrCode2=b0.OcrCode2 and b0.Tipo='A' and b1.DocDate < b0.DocDate
left join
(
select a1.DocDate, A1.DocEntry, a0.Tipo, A0.OcrCode2 from
(select min(t0.DocEntry) Folio, t1.OcrCode2, 'A' Tipo
from OWTR T0
inner join wtr1 t1 on t1.DocEntry = t0.DocEntry
where t1.WhsCode ='MQHEOBRA' and T0.U_MQ_Rise = @FolioOri and t1.OcrCode2 is not null and t1.Quantity>0 and t0.CANCELED='N'
group by t1.OcrCode2) A0
inner join OWTR A1 on a1.DocEntry = A0.Folio) B2 on  B2.OcrCode2=b0.OcrCode2 and b0.Tipo='C' and b2.DocDate <= b0.DocDate
where (b1.DocEntry is null and b0.Tipo ='A') or (b2.DocEntry is null and b0.Tipo ='C')
) W1 on w1.DocEntry = w0.DocEntry and w1.OcrCode2 = w0.OcrCode2 and WW.U_PrcCode is null
left join wtr1 w2 on w2.DocEntry = w0.DocEntry and w0.Tipo = 'A' and w2.OcrCode2 = w0.OcrCode2
left join PCH1 w3 on w3.DocEntry = w0.DocEntry and w0.Tipo = 'C' and w3.OcrCode2 = w0.OcrCode2
inner join oitm w4 on w4.ItemCode = w2.ItemCode or w4.ItemCode =w3.ItemCode
inner join oitm w5 on w5.ItemCode = w0.OcrCode2
left join [@UG_GLO_EQUIPO] w6 on w6.Code = w4.U_GLO_Equipo or w6.Code = w5.U_GLO_Equipo
where W1.DocEntry is null
group by w0.OcrCode2, w0.Tipo, w6.U_TypeEQ
