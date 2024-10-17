using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Robot robot1 = new Bos_Robot("Vexana", 100, 10, 15, 5);
        Robot robot2 = new Bos_Robot("Dyrroth", 120, 12, 18, 6);

        IKemampuan perbaikan = new Perbaikan(3);
        IKemampuan seranganListrik = new SeranganListrik(2);
        IKemampuan seranganPlasma = new SeranganPlasma(4);
        IKemampuan pertahananSuper = new PertahananSuper(5);

        robot1.Serang(robot2);
        robot2.GunakanKemampuan(seranganPlasma, robot1);
        robot1.GunakanKemampuan(perbaikan, robot1);

        robot1.Serang(robot2);
        robot2.GunakanKemampuan(pertahananSuper, robot2);
        robot1.GunakanKemampuan(seranganListrik, robot2);

        robot2.Serang(robot1);
    }
}

interface IKemampuan
{
    string Nama { get; }
    void Gunakan(Robot pengguna, Robot target);
    bool SiapDigunakan();
}

abstract class Robot
{
    public string nama;
    public int energi;
    public int armor;
    public int serangan;

    public Robot(string nama, int energi, int armor, int serangan)
    {
        this.nama = nama;
        this.energi = energi;
        this.armor = armor;
        this.serangan = serangan;
    }

    public void Serang(Robot target)
    {
        int damage = serangan - target.armor;
        damage = damage < 0 ? 0 : damage; 
        target.energi -= damage;
        Console.WriteLine($"{nama} menyerang {target.nama}, menyebabkan {damage} damage!");
        target.CetakInformasi();
    }

    public abstract void GunakanKemampuan(IKemampuan kemampuan, Robot target);

    public void CetakInformasi()
    {
        Console.WriteLine($"{nama}: Energi = {energi}, Armor = {armor}, Serangan = {serangan}");
    }

    public bool Mati()
    {
        return energi <= 0;
    }
}

class Bos_Robot : Robot
{
    public int pertahanan;

    public Bos_Robot(string nama, int energi, int armor, int serangan, int pertahanan) : base(nama, energi, armor, serangan)
    {
        this.pertahanan = pertahanan;
    }

    public void Diserang(Robot penyerang)
    {
        int damage = penyerang.serangan - (armor + pertahanan);
        damage = damage < 0 ? 0 : damage;
        energi -= damage;
        Console.WriteLine($"{nama} diserang oleh {penyerang.nama}, menerima {damage} damage!");
        if (Mati())
        {
            Console.WriteLine($"{nama} telah mati!");
        }
    }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        if (kemampuan.SiapDigunakan())
        {
            kemampuan.Gunakan(this, target);
        }
        else
        {
            Console.WriteLine($"{nama} tidak bisa menggunakan kemampuan {kemampuan.Nama} karena cooldown.");
        }
    }
}

class Perbaikan : IKemampuan
{
    private int _cooldown;
    private int _cooldownSekarang;

    public string Nama => "Perbaikan";

    public Perbaikan(int cooldown)
    {
        _cooldown = cooldown;
        _cooldownSekarang = 0;
    }

    public void Gunakan(Robot pengguna, Robot target)
    {
        pengguna.energi += 20;
        Console.WriteLine($"{pengguna.nama} menggunakan {Nama}, memulihkan 20 energi.");
        _cooldownSekarang = _cooldown;
    }

    public bool SiapDigunakan()
    {
        if (_cooldownSekarang > 0)
        {
            _cooldownSekarang--;
            return false;
        }
        return true;
    }
}

class SeranganListrik : IKemampuan
{
    private int _cooldown;
    private int _cooldownSekarang;

    public string Nama => "Serangan Listrik";

    public SeranganListrik(int cooldown)
    {
        _cooldown = cooldown;
        _cooldownSekarang = 0;
    }

    public void Gunakan(Robot pengguna, Robot target)
    {
        target.energi -= 15;
        Console.WriteLine($"{pengguna.nama} menggunakan {Nama}, menyerang {target.nama} dengan listrik, mengurangi 15 energi!");
        _cooldownSekarang = _cooldown;
    }

    public bool SiapDigunakan()
    {
        if (_cooldownSekarang > 0)
        {
            _cooldownSekarang--;
            return false;
        }
        return true;
    }
}

class SeranganPlasma : IKemampuan
{
    private int _cooldown;
    private int _cooldownSekarang;

    public string Nama => "Serangan Plasma";

    public SeranganPlasma(int cooldown)
    {
        _cooldown = cooldown;
        _cooldownSekarang = 0;
    }

    public void Gunakan(Robot pengguna, Robot target)
    {
        int damage = pengguna.serangan * 2; 
        target.energi -= damage;
        Console.WriteLine($"{pengguna.nama} menggunakan {Nama}, menyebabkan {damage} damage kepada {target.nama}!");
        _cooldownSekarang = _cooldown;
    }

    public bool SiapDigunakan()
    {
        if (_cooldownSekarang > 0)
        {
            _cooldownSekarang--;
            return false;
        }
        return true;
    }
}

class PertahananSuper : IKemampuan
{
    private int _cooldown;
    private int _cooldownSekarang;

    public string Nama => "Pertahanan Super";

    public PertahananSuper(int cooldown)
    {
        _cooldown = cooldown;
        _cooldownSekarang = 0;
    }

    public void Gunakan(Robot pengguna, Robot target)
    {
        pengguna.armor += 10;
        Console.WriteLine($"{pengguna.nama} menggunakan {Nama}, meningkatkan armor sebesar 10 untuk sementara.");
        _cooldownSekarang = _cooldown;
    }

    public bool SiapDigunakan()
    {
        if (_cooldownSekarang > 0)
        {
            _cooldownSekarang--;
            return false;
        }
        return true;
    }
}


