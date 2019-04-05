--Empleado por facturas

declare @CommissionID varchar(50);

set @CommissionID = '{Folio}'
	 select 
		'INV' as Type, 
		OINV.DocEntry as Id, 
		OINV.DocNum as DocNum,
		OINV.DocDate, 
		OHEM.empID as U_DriverId, 
		OSLP.SlpName U_Driver, 
		OITM.ItemName,
		INV1.U_TR_Paths as Route, 
		OOCR.OcrName as AF, 
		TYP.Name as TypLoad, 
		INV1.LineTotal,
		INV1.LineTotal * ([@UG_TR_VETY].U_Commission/100) as U_Commission,
		ISNULL((select A1.LineTotal from INV1 A1 with(nolock) where A1.DocEntry = INV1.DocEntry and A1.ItemCode =  (select U_Value from [@UG_CONFIG] with(nolock) where Name = 'TR_INSURANCE')), 0) as U_Seguro,
		ISNULL((select U_Value from [@UG_CONFIG] with(nolock) where Name = 'TR_LIQ_IMPORTE'), 0) as WkDisc
		
	from [@UG_TR_CMLN] CMLN with (nolock)
	inner join OINV with (nolock) on OINV.DocEntry = CMLN.U_DocEntry
	inner join OHEM with (nolock) on OHEM.empId = CMLN.U_DriverId
	inner join OSLP with (nolock) on OHEM.SalesPrson = OSLP.SlpCode
	inner join INV1 with (nolock) on INV1.DocEntry = OINV.DocEntry and INV1.SlpCode = OSLP.SlpCode
	inner join OITM  with(nolock) on OITM.ItemCode = INV1.ItemCode
	inner join OITB with(nolock) on OITB.ItmsGrpCod = OITM.ItmsGrpCod
	left join OOCR with(nolock) on OOCR.OcrCode = INV1.OcrCode2
	left join [@UG_TR_TRTY] TYP with (nolock) on TYP.Code = INV1.U_TR_LoadType
	left join [@UG_TR_VETY]  with(nolock) on [@UG_TR_VETY].U_EquipType = INV1.U_TR_VehicleType

	where U_Folio = @CommissionID 
	and CMLN.U_Type = 'INV'
	and (OITB.ItmsGrpNam = 'FLETES')
	and CMLN.U_Status != '3' --No cancleado
		union all
		--Consulta de asiento
	 select 
		'JDT' as Type,  
		OJDT.TransID as Id, 
		OJDT.TransID as DocNum, 
		OJDT.RefDate, 
		OHEM.empID,  
		OSLP.SlpName, 
		JDT1.LineMemo as ItemName,  
		JDT1.U_TR_Paths as Route,
		OOCR.OcrName as AF, 
		TYP.Name as TypLoad, 
		JDT1.Credit as LineTotal, 
		JDT1.Credit * (TVE.U_Commission/100) as U_Commission, 0 as U_Seguro,
		ISNULL((select U_Value from [@UG_CONFIG] with(nolock) where Name = 'TR_LIQ_IMPORTE'), 0) as WkDisc
		
	 from OHEM with (Nolock)
	
	 inner join OHPS  with (Nolock) on OHEM.Position = OHPS.posID
	 inner join OUDP with (Nolock) on OHEM.dept = OUDP.Code
	inner join OSLP with (nolock) on OHEM.SalesPrson = OSLP.SlpCode
	 inner join [@UG_TR_CMLN] CMLN with (nolock) on OHEM.empID = CMLN.U_DriverId
	inner join JDT1 with (nolock) on JDT1.U_GLO_Auxiliar = OHEM.empID and JDT1.TransID = CMLN.U_DocEntry
	inner join OJDT with (nolock) on OJDT.TransID = JDT1.TransID 
	full outer join OJDT T2 with (Nolock) on T2.StornoToTr = OJDT.TransId 
	inner join [@UG_Config] with (nolock) on [@UG_Config].U_Value = JDT1.Account
	inner join [@UG_TR_INTLFRGHT] TFL with (nolock) on OJDT.Ref1 = TFL.U_InternalFolio
	left join [@UG_TR_VETY] TVE with (nolock) on TVE.U_EquipType = TFL.U_VehicleType
	left join OOCR with(nolock) on OOCR.OcrCode = JDT1.OcrCode2
	left join [@UG_TR_TRTY] TYP with (nolock) on TYP.Code = TFL.U_PayloadType
	where 
		OHPS.Name = 'OPERADOR' 
		and OUDP.Name = 'TRANSPORTES' 
		and [@UG_Config].Name = 'TR_FLETE_INT'
		 and OJDT.AutoStorno='N' and T2.TransId is null and OJDT.StornoToTr is null
		 and CMLN.U_Folio = @CommissionID 
		 and CMLN.U_Status != '3' --No cancleado
		 and CMLN.U_Type = 'JDT'
		and OJDT.TransCode = 'TR/F' and OJDT.StornoToTr is null  