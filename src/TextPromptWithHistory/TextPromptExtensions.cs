namespace TextPromptWithHistory;

/// <summary>
/// Contains extension methods for <see cref="TextPrompt{T}"/>.
/// </summary>
public static class TextPromptExtensions
{
    /// <summary>
    /// Allow empty input.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> AllowEmpty<T>(this TextPromptWithHistory<T> obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.AllowEmpty = true;
        return obj;
    }

    /// <summary>
    /// Sets the prompt style.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="style">The prompt style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> PromptStyle<T>(this TextPromptWithHistory<T> obj, Style style)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (style is null)
        {
            throw new ArgumentNullException(nameof(style));
        }

        obj.PromptStyle = style;
        return obj;
    }

    /// <summary>
    /// Show or hide choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="show">Whether or not choices should be visible.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> ShowChoices<T>(this TextPromptWithHistory<T> obj, bool show)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.ShowChoices = show;
        return obj;
    }

    /// <summary>
    /// Shows choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> ShowChoices<T>(this TextPromptWithHistory<T> obj)
    {
        return ShowChoices(obj, true);
    }

    /// <summary>
    /// Hides choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> HideChoices<T>(this TextPromptWithHistory<T> obj)
    {
        return ShowChoices(obj, false);
    }

    /// <summary>
    /// Show or hide the default value.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="show">Whether or not the default value should be visible.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> ShowDefaultValue<T>(this TextPromptWithHistory<T> obj, bool show)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.ShowDefaultValue = show;
        return obj;
    }

    /// <summary>
    /// Shows the default value.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> ShowDefaultValue<T>(this TextPromptWithHistory<T> obj)
    {
        return ShowDefaultValue(obj, true);
    }

    /// <summary>
    /// Hides the default value.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> HideDefaultValue<T>(this TextPromptWithHistory<T> obj)
    {
        return ShowDefaultValue(obj, false);
    }

    /// <summary>
    /// Sets the validation error message for the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="message">The validation error message.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> ValidationErrorMessage<T>(this TextPromptWithHistory<T> obj, string message)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.ValidationErrorMessage = message;
        return obj;
    }

    /// <summary>
    /// Sets the "invalid choice" message for the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="message">The "invalid choice" message.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> InvalidChoiceMessage<T>(this TextPromptWithHistory<T> obj, string message)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.InvalidChoiceMessage = message;
        return obj;
    }

    /// <summary>
    /// Sets the default value of the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="value">The default value.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> DefaultValue<T>(this TextPromptWithHistory<T> obj, T value)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.DefaultValue = new DefaultPromptValue<T>(value);
        return obj;
    }

    /// <summary>
    /// Sets the validation criteria for the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="validator">The validation criteria.</param>
    /// <param name="message">The validation error message.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> Validate<T>(this TextPromptWithHistory<T> obj, Func<T, bool> validator, string? message = null)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Validator = result =>
        {
            if (validator(result))
            {
                return ValidationResult.Success();
            }

            return ValidationResult.Error(message);
        };

        return obj;
    }

    /// <summary>
    /// Sets the validation criteria for the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="validator">The validation criteria.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> Validate<T>(this TextPromptWithHistory<T> obj, Func<T, ValidationResult> validator)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Validator = validator;

        return obj;
    }

    /// <summary>
    /// Adds a choice to the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="choice">The choice to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> AddChoice<T>(this TextPromptWithHistory<T> obj, T choice)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Choices.Add(choice);
        return obj;
    }

    /// <summary>
    /// Adds multiple choices to the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="choices">The choices to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> AddChoices<T>(this TextPromptWithHistory<T> obj, IEnumerable<T> choices)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (choices is null)
        {
            throw new ArgumentNullException(nameof(choices));
        }

        foreach (var choice in choices)
        {
            obj.Choices.Add(choice);
        }

        return obj;
    }

    /// <summary>
    /// Replaces prompt user input with asterisks in the console.
    /// </summary>
    /// <typeparam name="T">The prompt type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> Secret<T>(this TextPromptWithHistory<T> obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.IsSecret = true;
        return obj;
    }

    /// <summary>
    /// Replaces prompt user input with mask in the console.
    /// </summary>
    /// <typeparam name="T">The prompt type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="mask">The masking character to use for the secret.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> Secret<T>(this TextPromptWithHistory<T> obj, char? mask)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.IsSecret = true;
        obj.Mask = mask;
        return obj;
    }

    /// <summary>
    /// Sets the function to create a display string for a given choice.
    /// </summary>
    /// <typeparam name="T">The prompt type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="displaySelector">The function to get a display string for a given choice.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> WithConverter<T>(this TextPromptWithHistory<T> obj, Func<T, string>? displaySelector)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Converter = displaySelector;
        return obj;
    }

    /// <summary>
    /// Sets the style in which the default value is displayed.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="style">The default value style or <see langword="null"/> to use the default style (green).</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> DefaultValueStyle<T>(this TextPromptWithHistory<T> obj, Style? style)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.DefaultValueStyle = style;
        return obj;
    }

    /// <summary>
    /// Sets the style in which the list of choices is displayed.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="style">The style to use for displaying the choices or <see langword="null"/> to use the default style (blue).</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> ChoicesStyle<T>(this TextPromptWithHistory<T> obj, Style? style)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.ChoicesStyle = style;
        return obj;
    }

    /// <summary>
    /// Adds history to the prompt to allow arrow-key scrolling on input.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="history">The history lines to add.  The last history string will be the first shown when the user arrows up at the prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPromptWithHistory<T> AddHistory<T>(this TextPromptWithHistory<T> obj, IEnumerable<string> history)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (history is null)
        {
            throw new ArgumentNullException(nameof(history));
        }

        obj.History.AddRange(history);

        return obj;
    }
}