using System;

public class Parameter
{
	public string parameter { get; private set; }
	public object parameterValue { get; private set; }
	public Parameter(string parameter, object parameterValue)
	{
		this.parameter = parameter;
		this.parameterValue = parameterValue;
	}
}
