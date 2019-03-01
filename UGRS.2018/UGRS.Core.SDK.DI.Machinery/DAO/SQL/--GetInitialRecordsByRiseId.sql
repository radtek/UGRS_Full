declare @FolioOri nvarchar(10)
set @FolioOri= '{RiseId}'

/*
= Obtener los primeros movimientos de esa subida
= Obtener los AF
select * from  [@UG_MQ_RIFR]
*/
select 
 R0.Equipo, r0.FrgnName NumEconomico,
isnull(R2.Code, '') Code,isnull(R2.U_IdRise, '') IdRise, r0.EqType, 
 case when R4.U_DieselM is null then isnull(R2.U_DieselM, 0) else R4.U_DieselM end DieselM, 
 case when R4.U_DieselT is null then isnull(R2.U_DieselT, 0) else R4.U_DieselT end DieselT,
case when R4.U_Gas is null then isnull(R2.U_Gas, 0) else R4.U_Gas end Gasolina,
case when R4.U_F15W40 is null then isnull(r1.[15W40], 0) else R4.U_F15W40 end [15W40], 
 case when R4.U_Hidraulic is null then isnull(r1.Hidraulico, 0) else R4.U_Hidraulic end Hidraulico, 
 case when R4.U_SAE40 is null then isnull(r1.SAE_40, 0) else R4.U_SAE40 end SAE_40,
case when R4.U_Transmition is null then isnull(r1.Transm, 0) else R4.U_Transmition end Transm, 
 case when R4.U_Oils is null then isnull(r1.Grasa, 0) else R4.U_Oils end Grasa, 
 case when R4.U_KmHr is null then isnull(R2.U_KmHr, 0) else R4.U_KmHr end 'Km/Hr'
from 
 ( Select l0.Itemcode Equipo, l1.FrgnName, l2.U_TypeEQ EqType from 
  ( ---- Lista de Todos los AF usados en la Subida
       select a0.OcrCode2 Itemcode from (
                select T1.OcrCode2 from OPCH T0 with (nolock) inner join PCH1 T1 with (nolock) on t0.DocEntry= t1.DocEntry where T1.OcrCode2 is not null and T0.U_MQ_Rise =@FolioOri and t1.Quantity>0 and t0.CANCELED='N' union all
                select T1.OcrCode2 from OWTR T0 with (nolock) inner join WTR1 T1 with (nolock) on t0.DocEntry= t1.DocEntry where T1.OcrCode2 is not null and T0.U_MQ_Rise =@FolioOri and t1.Quantity>0 and t0.CANCELED='N' union all
                select T1.OcrCode2 from OWTQ T0 with (nolock) inner join WTQ1 T1 with (nolock) on t0.DocEntry= t1.DocEntry where T1.OcrCode2 is not null and T0.U_MQ_Rise =@FolioOri and t1.Quantity>0 union all
                ---- Anexa los AF relacionados
                select T2.U_PrcCode from [@UG_TBL_MQ_RISE] T0 with (nolock) inner join [@UG_TBL_MQ_RISE] T1 with (nolock) on t0.U_DocRef = t1.U_IdRise inner join [@UG_MQ_RIFR] T2 with (nolock) on t1.U_IdRise= t2.U_IdRise where T0.U_IdRise =@FolioOri
         ) A0
         group by a0.OcrCode2
  ) L0  
       inner join oitm L1 with (nolock) on L1.ItemCode = L0.Itemcode 
       inner join [@UG_GLO_EQUIPO] L2 with (nolock) on L2.Code = L1.U_GLO_Equipo
) R0
left join (select 
 F0.OcrCode2 Equipo,
sum(case when F3.QryGroup26='Y' then case when f1.DocEntry is not null then f1.Quantity else f2.Quantity end else 0 end) '15W40',
sum(case when F3.QryGroup28='Y' then case when f1.DocEntry is not null then f1.Quantity else f2.Quantity end else 0 end) Hidraulico,
sum(case when F3.QryGroup29='Y' then case when f1.DocEntry is not null then f1.Quantity else f2.Quantity end else 0 end) SAE_40,
sum(case when F3.QryGroup30='Y' then case when f1.DocEntry is not null then f1.Quantity else f2.Quantity end else 0 end) Transm,
sum(case when F3.QryGroup23='Y' then case when f1.DocEntry is not null then f1.Quantity else f2.Quantity end else 0 end) Grasa
from (
       ----- Obtener los DocEntrys iniciales de cada AF
       select 
              B0.DocEntry, B0.OcrCode2, B0.Tipo
       from (  ----- Universo de facturas y traslados de cada AF
                     ----- Obtiene la Fecha del traslado
                     select 
                              a1.DocDate, A1.DocEntry, a0.Tipo, A0.OcrCode2 
                      from
                     (----- Busca los primeros DocEntrys de los AF en los traslados
                           select 
                                   min(t0.DocEntry) Folio, t1.OcrCode2, 'T' Tipo
                           from OWTR T0 with (nolock)
                           inner join wtr1 t1 with (nolock) on t1.DocEntry = t0.DocEntry
                           where t1.WhsCode ='MQHEOBRA' and T0.U_MQ_Rise = @FolioOri and t1.OcrCode2 is not null and t1.Quantity>0 and t0.CANCELED='N'
                           group by t1.OcrCode2
                     ) A0
                     inner join OWTR A1 on a1.DocEntry = A0.Folio
              union all
                     ----- Obtiene la Fecha de la factura
                     select 
                           a1.DocDate, A1.DocEntry, a0.Tipo , A0.OcrCode2
                     from 
                     (----- Busca los primeros DocEntrys de los AF en las facturas
                           select 
                                  min(t0.DocEntry) Folio, 'C' Tipo , t1.OcrCode2 
                           from OPCH T0 with (nolock)
                           inner join PCH1 t1 with (nolock) on t0.DocEntry = t1.DocEntry
                           where t1.WhsCode ='MQHEOBRA' and t0.CANCELED ='N' and t0.U_MQ_Rise=@FolioOri and t1.OcrCode2 is not null
                           group by t1.OcrCode2
                     ) A0
                     inner join OPCH A1 on a0.Folio = a1.DocEntry
       ) B0
       left join
       ( ----- Listado de facturas para validarlo contra el universo de traslados en base a la fecha,
         ------ Compara si las compras son mas viejas que el primer traslado
              select 
                     a1.DocDate, A1.DocEntry, a0.Tipo , A0.OcrCode2
              from 
              (select 
                     min(t0.DocEntry) Folio, 'C' Tipo , t1.OcrCode2 
              from OPCH T0 with (nolock)
              inner join PCH1 t1 with (nolock) on t0.DocEntry = t1.DocEntry
              where t1.WhsCode ='MQHEOBRA' and t0.CANCELED ='N' and t0.U_MQ_Rise=@FolioOri and t1.OcrCode2 is not null
              group by t1.OcrCode2
              ) A0
              inner join OPCH A1 with (nolock) on a0.Folio = a1.DocEntry
       ) B1 on  b1.OcrCode2=b0.OcrCode2 and b0.Tipo='T' and b1.DocDate < b0.DocDate
       left join
         (----- Listado de traslados para validarlo contra el universo de facturas en base a la fecha,
          ------ Compara si los traslados son mas viejos que la primer factura
                     select 
                            a1.DocDate, A1.DocEntry, a0.Tipo, A0.OcrCode2 
                      from
                        (----- Busca los primeros DocEntrys de los AF en los traslados
                                  select 
                                         min(t0.DocEntry) DocEntry, t1.OcrCode2, 'T' Tipo
                                  from OWTR T0 with (nolock)
                                  inner join wtr1 t1 with (nolock) on t1.DocEntry = t0.DocEntry
                                  where t1.WhsCode ='MQHEOBRA' and T0.U_MQ_Rise = @FolioOri and t1.OcrCode2 is not null and t1.Quantity>0 and t0.CANCELED='N'
                                  group by t1.OcrCode2
                           ) A0
                     inner join OWTR A1 with (nolock) on a1.DocEntry = A0.DocEntry
              ) B2 on  B2.OcrCode2=b0.OcrCode2 and b0.Tipo='C' and b2.DocDate <= b0.DocDate
       where (b1.DocEntry is null and b0.Tipo ='T') or (b2.DocEntry is null and b0.Tipo ='C')
) F0
left join wtr1 F1 with (nolock) on F1.DocEntry = F0.DocEntry and f0.Tipo = 'T' and f1.OcrCode2 = f0.OcrCode2
left join PCH1 F2 with (nolock) on F2.DocEntry = F0.DocEntry and f0.Tipo = 'C' and f2.OcrCode2 = f0.OcrCode2
inner join oitm f3 with (nolock) on f3.ItemCode = f1.ItemCode or f3.ItemCode =f2.ItemCode
group by f0.OcrCode2--, f6.U_TypeEQ
) R1 on R1.Equipo = R0.Equipo
left join [@UG_MQ_RIIR] R2 with (nolock) on ltrim(rtrim(R2.U_PrcCode)) = R0.Equipo and ltrim(rtrim(R2.U_IdRise))= @FolioOri
left join [@UG_TBL_MQ_RISE] R3 with (nolock) on @FolioOri= ltrim(rtrim(R3.U_IdRise))
left join [@UG_MQ_RIFR] R4 with (nolock) on ltrim(rtrim(R4.U_PrcCode)) = R0.Equipo and ltrim(rtrim(R4.U_IdRise))= R3.U_DocRef