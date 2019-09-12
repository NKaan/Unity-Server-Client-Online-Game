using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ByteBuffer : IDisposable //Tek Kullanımlık
{
    List<byte> Buffer; //Buffer
    byte[] okunan_buffer;
    int okunanPos;
    bool buffer_guncelle = false;

    public ByteBuffer()
    {
        Buffer = new List<byte>();
        okunanPos = 0;
    }

    public void Byte_Yaz(byte Girisler)
    {
        Buffer.Add(Girisler);
        buffer_guncelle = true;
    }

    public void Bytes_Yaz(byte[] Giris)
    {
        Buffer.AddRange(Giris);
        buffer_guncelle = true;
    }

    public void Short_Yaz(short Giris)
    {
        Buffer.AddRange(BitConverter.GetBytes(Giris));
        buffer_guncelle = true;
    }

    public void Int_Yaz(int Giris)
    {
        Buffer.AddRange(BitConverter.GetBytes(Giris));
        buffer_guncelle = true;
    }

    public void Float_Yaz(float Giris)
    {
        Buffer.AddRange(BitConverter.GetBytes(Giris));
        buffer_guncelle = true;
    }

    public void String_Yaz(string Giris)
    {
        Buffer.AddRange(BitConverter.GetBytes(Giris.Length));
        Buffer.AddRange(Encoding.ASCII.GetBytes(Giris));
        buffer_guncelle = true;
    }





    public string String_Oku(bool Dikiz = true)
    {
        int len = Int_Oku(true);
        if (buffer_guncelle)
        {
            okunan_buffer = Buffer.ToArray();
            buffer_guncelle = false;
        }

        string ret = Encoding.ASCII.GetString(okunan_buffer, okunanPos, len);
        if (Dikiz & Buffer.Count > okunanPos)
        {
            if (ret.Length > 0)
            {
                okunanPos += len;
            }
        }
        return ret;
    }

    public byte Byte_Oku(bool Dikiz = true)
    {
        if (Buffer.Count > okunanPos)
        {
            if (buffer_guncelle)
            {
                okunan_buffer = Buffer.ToArray();
                buffer_guncelle = false;
            }

            byte ret = okunan_buffer[okunanPos];
            if (Dikiz & Buffer.Count > okunanPos)
            {
                okunanPos += 1;
            }
            return ret;
        }

        else
        {
            throw new Exception("Bayt Arabelleğini Geçtiniz");
        }
    }

    public byte[] Bytes_Oku(int Length, bool Dikiz = true)
    {
        if (buffer_guncelle)
        {
            okunan_buffer = Buffer.ToArray();
            buffer_guncelle = false;
        }

        byte[] ret = Buffer.GetRange(okunanPos, Length).ToArray();
        if (Dikiz)
        {
            okunanPos += Length;
        }
        return ret;
    }

    public float Float_Oku(bool Dikiz = true)
    {
        if (Buffer.Count > okunanPos)
        {
            if (buffer_guncelle)
            {
                okunan_buffer = Buffer.ToArray();
                buffer_guncelle = false;
            }

            float ret = BitConverter.ToSingle(okunan_buffer, okunanPos);
            if (Dikiz & Buffer.Count > okunanPos)
            {
                okunanPos += 4;
            }
            return ret;
        }

        else
        {
            throw new Exception("Bayt Arabelleğini Geçtiniz");
        }
    }

    public int Int_Oku(bool Dikiz = true)
    {
        if (Buffer.Count > okunanPos)
        {
            if (buffer_guncelle)
            {
                okunan_buffer = Buffer.ToArray();
                buffer_guncelle = false;
            }

            int ret = BitConverter.ToInt32(okunan_buffer, okunanPos);
            if (Dikiz & Buffer.Count > okunanPos)
            {
                okunanPos += 4;
            }
            return ret;
        }

        else
        {
            throw new Exception("Bayt Arabelleğini Geçtiniz");
        }
    }

    public int GetReadPos()
    {
        return okunanPos;
    }

    public byte[] ToArray()
    {
        return Buffer.ToArray();
    }

    public int Count()
    {
        return Buffer.Count;
    }

    public int Length()
    {
        return Count() - okunanPos;
    }

    public void Clear()
    {
        Buffer.Clear();
        okunanPos = 0;
    }

    private bool cikarilan_deger = false;


    //Tek Kullanımlık
    protected virtual void Dispose(bool disposing)
    {
        if (!this.cikarilan_deger)
        {
            if (disposing)
            {
                Buffer.Clear();
            }

            okunanPos = 0;
        }
        this.cikarilan_deger = true;
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}