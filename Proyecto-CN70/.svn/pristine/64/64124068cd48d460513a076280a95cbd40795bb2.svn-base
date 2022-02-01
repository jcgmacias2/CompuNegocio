using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Aprovi.Business.Services
{
    public abstract class BasculaService : IBasculaService
    {
        private IUnitOfWork _UOW;
        private IBasculasRepository _scales;
        private IAplicacionesRepository _app;

        public BasculaService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _scales = _UOW.Basculas;
            _app = _UOW.Aplicaciones;
        }

        public Bascula GetDefault(Estacione station)
        {
            try
            {
                return _scales.Find(station.idEstacion);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Bascula Update(Bascula scale)
        {
            try
            {
                var local = _scales.Find(scale.idBasculaEstacion);
                local.bitsDeDatos = scale.bitsDeDatos;
                local.bitsDeParada = scale.bitsDeParada;
                local.finDeLinea = scale.finDeLinea;
                local.paridad = scale.paridad;
                local.puerto = scale.puerto;
                local.tiempoDeEscritura = scale.tiempoDeEscritura;
                local.tiempoDeLectura = scale.tiempoDeLectura;
                local.velocidad = scale.velocidad;

                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePLUs(Bascula scale, List<Articulo> items)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string WritePLUs(Bascula scale, List<Articulo> items)
        {
            try
            {
                var file = string.Format("{0}\\Catálogo.txt",_app.ReadSetting("Reports"));

                if (File.Exists(file))
                    File.Delete(file);                

                //Solo voy a crear el archivo de texto con el catálogo de PLU's
                foreach (var a in items)
                {
                    //0x0C = \f 
                    //0x13 = \r Carriage Return
                    //0x10 = \n Line Feed
                    //  = \t Tab

                    //Enumerador = valor de instrucción
                    //0 : PLU
                    //1 : Descripción (hasta 52 caracteres)
                    //2 : Precio 6 caracteres máximo con punto decimal
                    //3 : Tipo (sin tipo)
                    //4 : PLU
                    //5 : Grupo 2 caracteres máximo, 0-99  (sin grupo)
                    //6 : Departamento 2 caracteres máximo, 0-99 (sin departamento)
                    //7 : Impuestos 3 caracteres como máximo, 0-100 (Por el momento 0)
                    //8 : Código (sin código ) 0


                    var instruction = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\r\n", a.codigo.ToString(), a.descripcion.Length > 52 ? a.descripcion.Substring(0, 52) : a.descripcion, (a.costoUnitario + ((a.Precios.First().utilidad / 100) * a.costoUnitario)).ToDecimalString(), 1, a.codigo.ToString(), 0, 0, a.Impuestos.Count > 0 ? a.Impuestos.First().valor.ToDecimalString() : "0", 0);
                    File.AppendAllText(file, instruction);
                }

                return file;

                //if (scale.puerto.Equals("TCP"))
                //    WriteInTCP(scale, items);
                //else
                //    WriteInSerial(scale, items);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsReady(Bascula scale)
        {
            try
            {
                SerialPort port;

                port = new SerialPort(scale.puerto, scale.velocidad, (Parity)scale.paridad, scale.bitsDeDatos, (StopBits)scale.bitsDeParada);
                port.ReadTimeout = scale.tiempoDeLectura;
                port.WriteTimeout = scale.tiempoDeEscritura;
                port.NewLine = scale.finDeLinea;

                port.Open();
                port.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void WriteInTCP(Bascula scale, List<Articulo> items)
        {
            try
            {
                TcpClient port;
                string result;
                Byte[] data;
                NetworkStream stream;
                int bytes;

                port = new TcpClient(scale.finDeLinea, scale.velocidad);
                stream = port.GetStream();

                foreach (var a in items)
                {
                    //12 bytes
                    //1 al 6 = Dirección en memorio del registro
                    //7 al 7 = Acción (G: Guardar, L: Leer, R: Reporte)
                    //8 al 8 = Tipo de registro (P: PLU's, p: precio)
                    //9 al 12 = Datos separados por tab (tab = \t)
                    //0x0C = \f 
                    //0x13 = \r Carriage Return

                    //Enumerador = valor de instrucción
                    //0 : Dirección en memoria
                    //1 : Datos vacios
                    // De aqui en adelante todos los datos se separan con un tab \t
                    //2 : Nombre hasta 52 caracteres
                    //3 : Precio 6 caracteres máximo con punto decimal
                    //4 : Tipo (sin tipo)
                    //5 : Grupo 2 caracteres máximo, 0-99  (sin grupo)
                    //6 : Departamento 2 caracteres máximo, 0-99 (sin departamento)
                    //7 : Impuestos 3 caracteres como máximo, 0-100 (Por el momento 0)
                    //8 : Referencia de tara (sin referencia)
                    //9 : Fecha de caducidad (sin fecha)
                    //10 : Referencia de ingredientes (sin referencia)
                    //11 : Número de PLU (código de artículo)
                    //12 : Código (código de artículo)
                    //13 : User1 (sin usuario)
                    //14 : User2 (sin usuario)
                    //15 : User3 (sin usuario)
                    var instruction = string.Format("{0}GP{1}\f\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t\r", a.idArticulo.ToString().PadLeft(6, '0'), "0\t0\t0\t", a.descripcion.Length > 52 ? a.descripcion.Substring(0, 52) : a.descripcion, (a.costoUnitario + ((a.Precios.First().utilidad / 100) * a.costoUnitario)).ToDecimalString(), 0, 0, 0, a.Impuestos.Count > 0 ? a.Impuestos.First().valor.ToDecimalString() : "0", 0, 0, 0, a.codigo, a.codigo, 0, 0, 0);
                    data = Encoding.ASCII.GetBytes(instruction);
                    stream.Write(data, 0, data.Length);

                    data = new Byte[256];
                    result = string.Empty;
                    bytes = stream.Read(data, 0, data.Length);
                    result = Encoding.ASCII.GetString(data, 0, bytes);
                    //Aqui puedo leer lo que me responde
                }

                stream.Close();
                port.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void WriteInSerial(Bascula scale, List<Articulo> items)
        {
            try
            {
                SerialPort port;
                string result;

                port = new SerialPort(scale.puerto, scale.velocidad, (Parity)scale.paridad, scale.bitsDeDatos, (StopBits)scale.bitsDeParada);
                port.ReadTimeout = scale.tiempoDeLectura;
                port.WriteTimeout = scale.tiempoDeEscritura;
                port.NewLine = scale.finDeLinea;

                port.Open();

                foreach (var a in items)
                {
                    //12 bytes
                    //1 al 6 = Dirección en memorio del registro
                    //7 al 7 = Acción (G: Guardar, L: Leer, R: Reporte)
                    //8 al 8 = Tipo de registro (P: PLU's, p: precio)
                    //9 al 12 = Datos separados por tab (tab = \t)
                    //0x13 = \r Carriage Return

                    //Enumerador = valor de instrucción
                    //0 : Dirección en memoria
                    //1 : Datos vacios
                    // De aqui en adelante todos los datos se separan con un tab \t
                    //2 : Nombre hasta 52 caracteres
                    //3 : Precio 6 caracteres máximo con punto decimal
                    //4 : Tipo (sin tipo)
                    //5 : Grupo 2 caracteres máximo, 0-99  (sin grupo)
                    //6 : Departamento 2 caracteres máximo, 0-99 (sin departamento)
                    //7 : Impuestos 3 caracteres como máximo, 0-100 (Por el momento 0)
                    //8 : Referencia de tara (sin referencia)
                    //9 : Fecha de caducidad (sin fecha)
                    //10 : Referencia de ingredientes (sin referencia)
                    //11 : Número de PLU (código de artículo)
                    //12 : Código (código de artículo)
                    //13 : User1 (sin usuario)
                    //14 : User2 (sin usuario)
                    //15 : User3 (sin usuario)
                    var instruction = string.Format("{0}GP{1}\f\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t\r", a.idArticulo.ToString().PadLeft(6, '0'), "X\tX\tX\t", a.descripcion.Length > 52 ? a.descripcion.Substring(0, 52) : a.descripcion, (a.costoUnitario + ((a.Precios.First().utilidad / 100) * a.costoUnitario)).ToDecimalString(), 0, 0, 0, a.Impuestos.Count > 0 ? a.Impuestos.First().valor.ToDecimalString() : "0", 0, 0, 0, a.codigo, a.codigo, 0, 0, 0);
                    port.Write(instruction);
                    result = port.ReadLine();
                }
                
                port.Close();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
