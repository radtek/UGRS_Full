	SELECT  
	C0.WhsCode                [Corral],
	C0.Quantity               [Existence],
	C0.DistNumber             [Batch], 
			
				CASE WHEN C0.InvoiceDate IS NOT NULL THEN 
					DATEDIFF(DAY,C0.InvoiceDate,GETDATE()) 
				ELSE 
					DATEDIFF(DAY,C0.CreateDate,GETDATE()) +1
				END [TotalDays],
				0 [DocEntry]
	FROM (
			SELECT  T1.DistNumber, T0.Quantity, t1.InDate CreateDate, T1.MnfSerial CardCode, T2.WhsCode,T6.ItemName,T6.ItemCode,
				(SELECT max(A0.DocDate) InvoiceDate 
					FROM OINV A0 
					inner join INV1 A1 on A0.DocEntry= A1.DocEntry    
					WHERE A0.CANCELED = 'N' and A1.U_SU_BatchAuc = T1.DistNumber and
							A1.ItemCode = (SELECT U_Value FROM [@UG_CONFIG] WHERE Name='CU_GLO_ITEMPAYR')
								    
					) InvoiceDate
			FROM  dbo.OBTQ T0  
			inner join OBTN T1 on T0.SysNumber=T1.SysNumber and t0.ItemCode= t1.ItemCode
			inner join OWHS T2 on T2.WhsCode=T0.WhsCode
			inner join OLCT T3 on T3.Code = T2.Location
			inner join OCRD T5 on T5.CardCode = T1.MnfSerial
			inner join OITM T6 on T6.ItemCode = T0.ItemCode
			WHERE t3.Location = '{Location}' and T0.Quantity>0 and T5.CardCode='{CardCode}'

	) C0 
	-- OBTIENE EL PRECIO POR EL ARTICULO DE COBRO DE PISO
	LEFT JOIN (
		SELECT A1.ItemCode, A1.Price FROM OPLN A0
		inner join ITM1 A1 on A0.ListNum = A1.PriceList

		where --A0.U_GLO_WhsCode='{Location}'
		 A0.U_GLO_Location = '{Location}'
		 and a1.ItemCode = (select U_Value from [@UG_CONFIG] where Name='CU_GLO_ITEMPAYR')
	) T3 on 1=1
	-- OBTIENE EL EL IMPUESTO APLICADO AL ARTICULO DE COBRO DE PISO
	LEFT JOIN (
		SELECT A1.Rate FROM OTCX A0
		INNER JOIN OSTC A1 ON A0.LnTaxCode = A1.Code
		WHERE BUSAREA =0 AND StrVal1 = (SELECT U_Value FROM [@UG_CONFIG] WHERE Name='CU_GLO_ITEMPAYR')
	) T4 on 1=1
	WHERE CASE WHEN C0.InvoiceDate IS NOT NULL THEN 
					DATEDIFF(DAY,C0.InvoiceDate,GETDATE()) 
				ELSE 
					case when DATEDIFF(DAY,C0.CreateDate,GETDATE()) = 0 then 1 
					else DATEDIFF(DAY,C0.CreateDate,GETDATE()) end
				END  >0

