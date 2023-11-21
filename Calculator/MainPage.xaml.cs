
using System.Text.RegularExpressions;

namespace Calculator;

public partial class MainPage : ContentPage
{
	bool isExpanding = false;
	double num1 = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnButtosHandler(object sender, EventArgs e)
	{
		Button button = (Button)sender;
		string buttonValue = button.Text;
		switch(buttonValue){
			case "CE":
				ResultEntry.Text = "";
				break;
			case "C":
				ResultEntry.Text = ResultEntry.Text[..^1];
				break;
			case "=":
				RunOperation();
				break;
			default:
				ResultEntry.Text += buttonValue;
				break;
		}

	}

	private void RunOperation()
	{
		if(isExpanding)
		{
			Exp();
			return;
		}
		BasicValidation(ResultEntry.Text);

		ResultEntry.Text = RPNCalculator.Calculate(ResultEntry.Text).ToString();
	}

	private void Submit(object sender, EventArgs e)
	{
		RunOperation();
	}

	private void Exp(object sender, EventArgs e)
	{
		Exp();
	}

	private void Exp()
	{
		string input = ResultEntry.Text;

		BasicValidation(input);

		if(isExpanding)
		{
			ResultEntry.Text = Math.Pow(num1, Converter(input)).ToString("F2");
			isExpanding = false;
		}else{
			num1 = Converter(input);
			ResultEntry.Text = "";
			isExpanding = true;
		}
	}
	

	private double Converter(string input)
	{
		if(!double.TryParse(input, out double num1)){
			DisplayAlert("Error", "Can not convert input to number ", "OK");
			return 0;
		}
		return num1;
	}

	private bool BasicValidation(string input)
	{
		string pattern = @"^[0-9()+*/.-]+$";
		bool isEntryValid = Regex.IsMatch(input, pattern);
		if (!isEntryValid)
		{
			DisplayAlert("Error", "Input has invalid symbols ", "OK");
			return false;
		}
		return true;
	}

}