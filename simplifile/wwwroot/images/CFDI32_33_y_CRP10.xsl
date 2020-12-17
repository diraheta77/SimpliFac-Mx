<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:cfdi="http://www.sat.gob.mx/cfd/3"  xmlns:tfd="http://www.sat.gob.mx/TimbreFiscalDigital" xmlns:pago10="http://www.sat.gob.mx/Pagos">
  <xsl:output method="html" encoding="UTF-8" indent="no"/>
  <xsl:template match="/">
    <html>
      <head>
        <style>
          @import url('https://fonts.googleapis.com/css?family=Roboto');
        </style>
        <title>COMPROBANTE FISCAL DIGITAL POR INTERNET (CFDI)</title>
        <style type="text/css">
          body {
          text-align:justify;
          font-size: 18px;
          font-weight:normal;
          background-color: #eee4e0;
          font-family: 'Roboto', sans-serif;
          }

          h1
          {
          font-size:26px;
          color:#333;
          font-weight: bold;
          margin-bottom: 5px;
          text-align: center;
          }

          .cuerpo_form {
          background-color:#fcfcfc;
          width:95%;
          margin:0 auto;
          padding: 10px 10px;
          }


          .bold { font-weight:bold; color:#cc480e;}

          table.gridtable {
          border-width: 1px;
          border-color: #cc480e;
          border-collapse: collapse;
          width:90%;
          margin-left:auto;
          margin-right:auto;
          }
          table.gridtable td {
          border-width: 1px;
          padding: 2px;
          border-style: solid;
          border-color: #cc480e;
          vertical-align: top;
          font-size: 12px;
          }
          .titulo{
          background-color:#e4815d;
          color:#333;
          font-weight:bold;
          }
          .subtitulo{
          background-color:#eee4e0;
          }
          .total{
          background-color:#feb65a;
          }
        </style>
      </head>
      <body>
        <xsl:for-each select="cfdi:Comprobante">

          <xsl:if test="@version = 3.2">

            <div class="texto_centro">
              <h1>
                CFDI Versión: <xsl:value-of select="@version"/>
                <br/>
                Factura
              </h1>
            </div>

            <div class="cuerpo_form">

              <table align="center" class="gridtable">
                <tr>
                  <td style="width:50%;">
                    <table style="width:100%;" class="datos">
                      <tr>
                        <td colspan="2" class="titulo">DATOS DEL EMISOR</td>
                      </tr>
                      <tr>
                        <td class="bold">RFC:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/@rfc"/>
                          <xsl:value-of select="cfdi:Emisor/@Rfc"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Nombre:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/@nombre"/>
                          <xsl:value-of select="cfdi:Emisor/@Nombre"/>
                        </td>
                      </tr>

                      <tr>
                        <td colspan="2" class="titulo subtitulo">DOMICILIO</td>
                      </tr>
                      <tr>
                        <td class="bold">Calle:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/cfdi:DomicilioFiscal/@calle"/>
                          <xsl:value-of select="cfdi:Emisor/cfdi:DomicilioFiscal/@Calle"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">No. Exterior:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/cfdi:DomicilioFiscal/@noExterior"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">No. Interior:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/cfdi:DomicilioFiscal/@noInterior"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Colonia:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/cfdi:DomicilioFiscal/@colonia"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Localidad:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/cfdi:DomicilioFiscal/@localidad"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Municipio:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/cfdi:DomicilioFiscal/@municipio"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Estado:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/cfdi:DomicilioFiscal/@estado"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Pais:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/cfdi:DomicilioFiscal/@pais"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">C.P.:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/cfdi:DomicilioFiscal/@codigoPostal"/>
                        </td>
                      </tr>

                      <tr>
                        <td colspan="2" class="titulo">DATOS DEL RECEPTOR</td>
                      </tr>
                      <tr>
                        <td class="bold">RFC:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/@rfc"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Nombre:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/@nombre"/>
                        </td>
                      </tr>

                      <tr>
                        <td colspan="2" class="titulo subtitulo">DOMICILIO</td>
                      </tr>
                      <tr>
                        <td class="bold">Calle:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/cfdi:Domicilio/@calle"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">No. Exterior:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/cfdi:Domicilio/@noExterior"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">No. Interior:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/cfdi:Domicilio/@noInterior"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Colonia:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/cfdi:Domicilio/@colonia"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Localidad:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/cfdi:Domicilio/@localidad"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Municipio:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/cfdi:Domicilio/@municipio"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Estado:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/cfdi:Domicilio/@estado"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Pais:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/cfdi:Domicilio/@pais"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">C.P.:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/cfdi:Domicilio/@codigoPostal"/>
                        </td>
                      </tr>

                    </table>
                  </td>
                  <td style="vertical-align:top;">
                    <table style="width:100%;" class="datos">
                      <tr>
                        <td colspan="2" class="titulo">DATOS DEL CFDI</td>
                      </tr>
                      <tr>
                        <td class="bold">Fecha:</td>
                        <td class="dato">
                          <xsl:value-of select="@fecha"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">UUID:</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Serie:</td>
                        <td class="dato">
                          <xsl:value-of select="@serie"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Folio:</td>
                        <td class="dato">
                          <xsl:value-of select="@folio"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Tipo:</td>
                        <td class="dato">
                          <xsl:value-of select="@tipoDeComprobante"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Forma de pago:</td>
                        <td class="dato">
                          <xsl:value-of select="@formaDePago" />
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Metodo de pago:</td>
                        <td class="dato">
                          <xsl:value-of select="@metodoDePago" />
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Moneda:</td>
                        <td class="dato">
                          <xsl:value-of select="@Moneda" />
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">No Certificado:</td>
                        <td class="dato">
                          <xsl:value-of select="@noCertificado" />
                        </td>
                      </tr>
                      <xsl:if test="cfdi:Impuestos/@totalImpuestosRetenidos" >
                        <tr>
                          <td colspan="2" class="titulo subtitulo">
                            IMPUESTOS RETENIDOS
                          </td>
                        </tr>
                        <xsl:for-each select="cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion">
                          <tr>
                            <td class="bold">Impuesto:</td>
                            <td class="dato">
                              <xsl:value-of select="@impuesto" />
                            </td>
                          </tr>
                          <tr>
                            <td class="bold">Importe:</td>
                            <td class="dato">
                              $ <xsl:value-of select="format-number(@importe, '###,###.00')" />
                            </td>
                          </tr>
                        </xsl:for-each>
                        <tr>
                          <td class="bold">Total:</td>
                          <td class="total">
                            $ <xsl:value-of select="format-number(cfdi:Impuestos/@totalImpuestosRetenidos, '###,###.00')"/>
                          </td>
                        </tr>
                      </xsl:if>
                      <xsl:if test="cfdi:Impuestos/@totalImpuestosTrasladados" >
                        <tr>
                          <td colspan="2" class="titulo subtitulo">
                            IMPUESTOS TRASLADADOS
                          </td>
                        </tr>
                        <xsl:for-each select="cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado">

                          <tr>
                            <td class="bold">Impuesto:</td>
                            <td class="dato">
                              <xsl:value-of select="@impuesto" />
                            </td>
                          </tr>
                          <tr>
                            <td class="bold">Tasa:</td>
                            <td class="dato">
                              <xsl:value-of select="@tasa" />
                            </td>
                          </tr>
                          <tr>
                            <td class="bold">Importe:</td>
                            <td class="dato">
                              $ <xsl:value-of select="format-number(@importe, '###,###.00')" />
                            </td>
                          </tr>

                        </xsl:for-each>

                        <tr>
                          <td class="bold">Total:</td>
                          <td class="total">
                            $ <xsl:value-of select="format-number(cfdi:Impuestos/@totalImpuestosTrasladados, '###,###.00')"/>
                          </td>
                        </tr>
                      </xsl:if>
                      <tr>
                        <td class="titulo subtitulo" colspan="2">TOTALES</td>
                      </tr>
                      <tr>
                        <td class="bold">SubTotal:</td>
                        <td class="dato">
                          $ <xsl:value-of select="format-number(@subTotal, '###,###.00')"/>
                        </td>
                      </tr>
                      <xsl:if test="@descuento" >
                        <tr>
                          <td class="bold">Descuento:</td>
                          <td class="dato">
                            $ <xsl:value-of select="format-number(@descuento, '###,###.00')"/>
                          </td>
                        </tr>
                      </xsl:if>
                      <tr>
                        <td class="bold">Total:</td>
                        <td class="total">
                          $ <xsl:value-of select="format-number(@total, '###,###.00')"/>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <xsl:if test="cfdi:Conceptos" >
                  <tr>
                    <td class="titulo" colspan="2">CONCEPTOS</td>
                  </tr>
                  <tr>
                    <td colspan="2">
                      <table style="width:100%;" id="conceptos" cellpadding="0" cellspacing="0">
                        <tr style="font-weight:bolder;">
                          <td class="titulo">NO.</td>
                          <td class="titulo">CANTIDAD</td>
                          <td class="titulo">UNIDAD</td>
                          <td class="titulo">DESCRIPCION</td>
                          <td class="titulo">VALOR UNITARIO</td>
                          <td class="titulo">IMPORTE</td>
                        </tr>
                        <xsl:for-each select="cfdi:Conceptos/cfdi:Concepto">
                          <tr>
                            <td>
                              <xsl:value-of select="@noIdentificacion"/>
                            </td>
                            <td>
                              <xsl:value-of select="@cantidad"/>
                            </td>
                            <td>
                              <xsl:value-of select="@unidad"/>
                            </td>
                            <td>
                              <xsl:value-of select="@descripcion"/>
                              <xsl:if test="cfdi:InformacionAduanera" >
                                <br/>
                                <p style="text-align:left;font-size:0.8em;">
                                  Inf. Aduanera<br/>
                                  - Numero: <xsl:value-of select="cfdi:InformacionAduanera/@numero"/><br/>
                                  - Fecha: <xsl:value-of select="cfdi:InformacionAduanera/@fecha"/><br/>
                                  - Aduana: <xsl:value-of select="cfdi:InformacionAduanera/@aduana"/>
                                </p>
                              </xsl:if>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(@valorUnitario, '###,###.00')"/>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(@importe, '###,###.00')"/>
                            </td>
                          </tr>
                        </xsl:for-each>
                      </table>
                    </td>
                  </tr>
                </xsl:if>
              </table>

            </div>
          </xsl:if>

          <!--AQUI COMIENZA CFDI 3.3 -->
          <!-- ******************************************** -->
          <!-- ******************************************** -->
          <!-- ******************************************** -->
          <xsl:if test="@Version = 3.3">

            <div class="texto_centro">
              <h1>
                CFDI Versión: <xsl:value-of select="@Version"/>
				<xsl:choose>
					<xsl:when test="cfdi:Complemento/pago10:Pagos/@Version = 1.0">
						<br/>Complemento para Recepción de Pagos
					</xsl:when>
					<xsl:otherwise>
						<br/>Factura
					</xsl:otherwise>
				</xsl:choose>
              </h1>
            </div>

            <div class="cuerpo_form">

              <table align="center" class="gridtable">
                <tr>
                  <td style="width:50%;">
                    <table style="width:100%;" class="datos">
                      <tr>
                        <td colspan="2" class="titulo">DATOS DEL EMISOR</td>
                      </tr>
                      <tr>
                        <td class="bold">RFC</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/@Rfc"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Nombre</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/@Nombre"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Régimen Fiscal</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Emisor/@RegimenFiscal"/>
                        </td>
                      </tr>
                      <tr>
                        <td colspan="2">
                          <br/>
                        </td>
                      </tr>
                      <tr>
                        <td colspan="2" class="titulo">DATOS DEL RECEPTOR</td>
                      </tr>
                      <tr>
                        <td class="bold">RFC</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/@Rfc"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Nombre</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/@Nombre"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Uso del CFDI</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Receptor/@UsoCFDI"/>
                        </td>
                      </tr>
                    </table>
                  </td>
                  <td style="vertical-align:top;width:50%;">
                    <table style="width:100%;" class="datos">
                      <tr>
                        <td colspan="2" class="titulo">DATOS DEL COMPROBANTE FISCAL</td>
                      </tr>
                      <tr>
                        <td class="bold">UUID</td>
                        <td class="dato">
                          <xsl:value-of select="cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Serie y Folio</td>
                        <td class="dato">
                          <xsl:value-of select="@Serie"/>
                          <xsl:value-of select="@Folio"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Fecha</td>
                        <td class="dato">
                          <xsl:value-of select="@Fecha"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Tipo de Comprobante</td>
                        <td class="dato">
                          <xsl:value-of select="@TipoDeComprobante"/>
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Forma de pago</td>
                        <td class="dato">
                          <xsl:value-of select="@FormaPago" />
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Metodo de pago</td>
                        <td class="dato">
                          <xsl:value-of select="@MetodoPago" />
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">Moneda</td>
                        <td class="dato">
                          <xsl:value-of select="@Moneda" />
                        </td>
                      </tr>
                      <tr>
                        <td class="bold">No Certificado</td>
                        <td class="dato">
                          <xsl:value-of select="@NoCertificado" />
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td colspan="2">
                    <br/>

                    <table style="width:100%;">
                      <tr>
                        <td colspan="3" class="titulo">IMPUESTOS Y TOTALES</td>
                      </tr>
                      <tr>
                        <td class="titulo subtitulo">
                          RETENIDOS
                        </td>
                        <td class="titulo subtitulo">
                          TRASLADADOS
                        </td>
                        <td class="titulo subtitulo">
                          TOTALES
                        </td>
                      </tr>
                      <tr>
                        <td>

                          <table style="width:100%;">
                            <xsl:for-each select="cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion">
                              <tr>
                                <td class="bold">Impuesto</td>
                                <td class="dato">
                                  <xsl:value-of select="@Impuesto" />
                                </td>
                              </tr>
                              <tr>
                                <td class="bold">Importe</td>
                                <td class="dato">
                                  $ <xsl:value-of select="format-number(@Importe, '###,###.00')" />
                                </td>
                              </tr>
                            </xsl:for-each>
                            <tr>
                              <td class="bold">Total</td>
                              <td class="total">
                                $ <xsl:value-of select="format-number(cfdi:Impuestos/@TotalImpuestosRetenidos, '###,###.00')"/>
                              </td>
                            </tr>
                          </table>

                        </td>

                        <td >

                          <table style="width:100%;">
                            <xsl:for-each select="cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado">
                              <tr>
                                <td class="bold">Impuesto</td>
                                <td class="dato">
                                  <xsl:value-of select="@Impuesto" />
                                </td>
                              </tr>
                              <tr>
                                <td class="bold">Tasa</td>
                                <td class="dato">
                                  <xsl:value-of select="@TasaOCuota" />
                                </td>
                              </tr>
                              <tr>
                                <td class="bold">Importe</td>
                                <td class="dato">
                                  $ <xsl:value-of select="format-number(@Importe, '###,###.00')" />
                                </td>
                              </tr>
                            </xsl:for-each>
                            <tr>
                              <td class="bold">Total</td>
                              <td class="total">
                                $ <xsl:value-of select="format-number(cfdi:Impuestos/@TotalImpuestosTrasladados, '###,###.00')"/>
                              </td>
                            </tr>
                          </table>

                        </td>

                        <td >

                          <table style="width:100%;">
                            <tr>
                              <td class="bold">SubTotal</td>
                              <td class="dato">
                                $ <xsl:value-of select="format-number(@SubTotal, '###,###.00')"/>
                              </td>
                            </tr>
                            <tr>
                              <xsl:if test="@Descuento" >
                                <td class="bold">Descuento</td>
                                <td class="dato">
                                  $ <xsl:value-of select="format-number(@Descuento, '###,###.00')"/>
                                </td>
                              </xsl:if>
                            </tr>
                            <tr>
                              <td class="bold">Total</td>
                              <td class="total">
                                $ <xsl:value-of select="format-number(@Total, '###,###.00')"/>
                              </td>
                            </tr>
                          </table>

                        </td>
                      </tr>

                    </table>

                    <br/>

                  </td>
                </tr>

                <xsl:if test="cfdi:Conceptos" >
                  <tr>
                    <td class="titulo" colspan="2">CONCEPTOS</td>
                  </tr>
                  <tr>
                    <td colspan="2">
                      <table style="width:100%;" id="conceptos" cellpadding="0" cellspacing="0">
                        <tr style="font-weight:bolder;">
                          <td class="titulo subtitulo">CLAVE</td>
                          <td class="titulo subtitulo">CANTIDAD</td>
                          <td class="titulo subtitulo">UNIDAD</td>
                          <td class="titulo subtitulo">DESCRIPCION</td>
                          <td class="titulo subtitulo">VALOR UNITARIO</td>
                          <td class="titulo subtitulo">IMPORTE</td>
                        </tr>
                        <xsl:for-each select="cfdi:Conceptos/cfdi:Concepto">
                          <tr>
                            <td>
                              <xsl:value-of select="@ClaveProdServ"/>
                            </td>
                            <td>
                              <xsl:value-of select="@Cantidad"/>
                            </td>
                            <td>
                              <xsl:value-of select="@ClaveUnidad"/>
                            </td>
                            <td>
                              <xsl:value-of select="@Descripcion"/>
                              <xsl:if test="cfdi:InformacionAduanera" >
                                <br/>
                                <p style="text-align:left;font-size:0.8em;">
                                  Inf. Aduanera<br/>
                                  - Numero: <xsl:value-of select="cfdi:InformacionAduanera/@numero"/><br/>
                                  - Fecha: <xsl:value-of select="cfdi:InformacionAduanera/@fecha"/><br/>
                                  - Aduana: <xsl:value-of select="cfdi:InformacionAduanera/@aduana"/>
                                </p>
                              </xsl:if>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(@ValorUnitario, '###,###.00')"/>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(@Importe, '###,###.00')"/>
                            </td>
                          </tr>
                        </xsl:for-each>
                      </table>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="cfdi:Complemento/pago10:Pagos/@Version = 1.0">
                  <tr>
                    <td class="" colspan="2"><br/></td>
                  </tr>
                  <tr>
                    <td class="titulo" colspan="2">COMPLEMENTO PARA PAGOS</td>
                  </tr>
                  <tr>
                    <td colspan="2">

                      <table style="width:100%;" id="conceptos" cellpadding="0" cellspacing="0">
                        <xsl:for-each select="cfdi:Complemento/pago10:Pagos/pago10:Pago">
                        <tr style="font-weight:bolder;">
                          <td class="titulo subtitulo">FECHA DE PAGO</td>
                          <td class="titulo subtitulo">FORMA DE PAGO</td>
                          <td class="titulo subtitulo">MONEDA</td>
                          <td class="titulo subtitulo">MONTO</td>
                        </tr>
                          <tr>
                            <td>
                              <xsl:value-of select="@FechaPago"/>
                            </td>
                            <td>
                              <xsl:value-of select="@FormaDePagoP"/>
                            </td>
                            <td>
                              <xsl:value-of select="@MonedaP"/>
                            </td>
                            <td>
                              $<xsl:value-of select="format-number(@Monto, '###,###.00')"/>
                            </td>
                          </tr>
                        <tr style="font-weight:bolder;">
                          <td class="titulo subtitulo" colspan="4">DOCUMENTO RELACIONADO</td>
                        </tr>
                        <xsl:for-each select="pago10:DoctoRelacionado">
                        <tr>
                          <td colspan="4">
                            <table style="width:100%;" id="conceptos" cellpadding="0" cellspacing="0">
                       <tr style="font-weight:bolder;">
                          <td class="titulo subtitulo">UUID DOCUMENTO</td>
                          <td class="titulo subtitulo">SERIE Y FOLIO</td>
                          <td class="titulo subtitulo">MONEDA</td>
                          <td class="titulo subtitulo">SALDO ANTERIOR</td>
                          <td class="titulo subtitulo">IMPORTE PAGADO</td>
                          <td class="titulo subtitulo">SALDO INSOLUTO</td>
                        </tr>

                       <tr>
                          <td><xsl:value-of select="@IdDocumento"/></td>
                          <td><xsl:value-of select="@Serie"/><xsl:value-of select="@Folio"/></td>
                          <td><xsl:value-of select="@MonedaDR"/></td>
                          <td><xsl:value-of select="@ImpSaldoAnt"/></td>
                          <td><xsl:value-of select="@ImpPagado"/></td>
                          <td><xsl:value-of select="@ImpSaldoInsoluto"/></td>
                        </tr>

                            </table>
                          </td>
                        </tr>
                        </xsl:for-each>
                        </xsl:for-each>
                      </table>

                    </td>
                  </tr>
                </xsl:if>

              </table>

            </div>
          </xsl:if>
          <!--AQUI FINALIZA CFDI 3.3 -->
          <!-- ******************************************** -->
          <!-- ******************************************** -->
          <!-- ******************************************** -->

        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>