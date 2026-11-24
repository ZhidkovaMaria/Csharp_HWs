using System;

public interface IDamageCalculator
{
    int CalculateDamage(int baseDamage, int targetArmor);
}

public class PhysicalDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        return Math.Max(1, baseDamage - targetArmor / 2);
    }
}

public class MagicDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        int damage = Math.Max(1, baseDamage - targetArmor / 4);

        if (targetArmor == 0)
        {
            damage *= 2;
        }

        return damage;
    }
}

public class FireDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        int damage = Math.Max(1, baseDamage - targetArmor / 10);

        if (baseDamage > 50)
        {
            damage += 10;
        }

        return damage;
    }
}

public class Unit
{
    public string Name { get; }
    public int HP { get; private set; }
    public int Armor { get; }

    public Unit(string name, int hp, int armor)
    {
        Name = name;
        HP = hp;
        Armor = armor;
    }

    public void TakeDamage(int amount)
    {
        HP = Math.Max(0, HP - amount);
    }
}

public class DamageSystem
{
    private readonly IDamageCalculator _damageCalculator;

    public DamageSystem(IDamageCalculator damageCalculator)
    {
        _damageCalculator = damageCalculator;
    }

    public void ApplyDamage(Unit target, int baseDamage)
    {
        int damage = _damageCalculator.CalculateDamage(baseDamage, target.Armor);
        target.TakeDamage(damage);
        Console.WriteLine($"Юнит {target.Name} получил {damage} урона. Осталось HP: {target.HP}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Unit unit = new Unit("Стражник", 200, 12);

        DamageSystem physicalSystem = new DamageSystem(new PhysicalDamageCalculator());
        DamageSystem magicSystem = new DamageSystem(new MagicDamageCalculator());
        DamageSystem fireSystem = new DamageSystem(new FireDamageCalculator());

        physicalSystem.ApplyDamage(unit, 60);
        magicSystem.ApplyDamage(unit, 60);
        fireSystem.ApplyDamage(unit, 60);
    }
}

// Чтобы добавить ледяной урон, достаточно создать класс IceDamageCalculator,
// реализующий IDamageCalculator, и передать его объект в конструктор DamageSystem.
// Менять DamageSystem не потребуется, потому что он зависит только от интерфейса.
