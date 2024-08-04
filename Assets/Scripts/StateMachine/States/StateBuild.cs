using System;
using UnityEngine;

public class StateBuild : State
{
    [SerializeField] private Settlement _settlementPrefab;

    private Employee _employee;

    protected override void InitializeAfterAddon()
    {
        if (AICharacter.TryGetComponent(out _employee) == false)
        {
            throw new ArgumentNullException(nameof(_employee));
        }
    }

    protected override void EnterAfterAddon()
    {
        if (_employee.Flag == null)
        {
            throw new InvalidOperationException($"Ошибка входа в состояние {nameof(StateBuild)}! Необходима инициализация {nameof(Flag)}");
        }
    }

    protected override void StartHandle()
    {
        Settlement settlement = Instantiate(_settlementPrefab, _employee.Flag.Position, _settlementPrefab.transform.rotation);
        _employee.Flag.Free();
        _employee.TransitSettlement(settlement);
    }

    protected override void Work() { }

    protected override bool CanHandle() => false;
}