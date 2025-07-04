namespace AplicacionTFG.Models;

public record Entity(string Name);

public record EntityNumber(int number);

public record EntityDate(DateOnly date);

public record EntityDateNumber(DateTime date, int number);
