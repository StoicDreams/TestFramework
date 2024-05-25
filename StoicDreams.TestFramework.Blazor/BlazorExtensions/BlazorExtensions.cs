using AngleSharp.Dom;
using System.Text.RegularExpressions;
using System.Web;

namespace StoicDreams.BlazorExtensions;

public static partial class BlazorExtensions
{
    /// <summary>
    /// Parses the rendered markup for any alerts and extracts each alert inner text to a line in the returned string.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="act"></param>
    /// <param name="alertCssSelector"></param>
    /// <returns></returns>

    public static string GetAlerts<TComponent>(this IRenderArrangement<TComponent> act, string alertCssSelector = ".mud-alert-message")
        where TComponent : IComponent
    {
        return string.Join(Environment.NewLine, act.Render.FindAll(alertCssSelector).Select(alert => alert.GetInnerText().Trim())).Trim();
    }

    /// <summary>
    /// <para>Build an exception message that outputs extra details (Exception message, current page, alerts, and markup).</para>
    /// <para>Note: Markup will have any style and script tags removed.</para>
    /// </summary>
    /// <param name="act"></param>
    /// <param name="message"></param>
    /// <param name="alertCssSelector"></param>
    /// <returns></returns>
    public static string BuildExceptionMessage<TComponent>(this IRenderArrangement<TComponent> act, string message, string alertCssSelector = ".mud-alert-message")
        where TComponent : IComponent
    {
        if (message.Contains("Markup:")) return message;
        return string.Join(Environment.NewLine, message, $"Page: {act.NavManager.GetRelativeUrl()}", "Alerts:", GetAlerts(act, alertCssSelector), "Markup:", act.Render.Markup.RemoveStyleHtml());
    }

    /// <summary>
    /// Get the current relative url of the page from the NavigationManager.
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    public static string GetCurrentPage<TComponent>(this IRenderArrangement<TComponent> act)
        where TComponent : IComponent => act.GetService<NavigationManager>().GetRelativeUrl();

    /// <summary>
    /// Get the current relative url.
    /// </summary>
    /// <param name="nav"></param>
    /// <returns></returns>
    public static string GetRelativeUrl(this NavigationManager nav) => HttpUtility.UrlDecode(nav.ToBaseRelativePath(nav.Uri));

    /// <summary>
    /// Remove any style tags from the HTML.
    /// </summary>
    /// <param name="markup"></param>
    /// <returns></returns>
    public static string RemoveStyleHtml(this string markup) => ScriptHtmlPattern().Replace(StyleHtmlPattern().Replace(markup, string.Empty), string.Empty);

    [GeneratedRegex(@"<style.*?</style>", RegexOptions.Singleline)]
    private static partial Regex StyleHtmlPattern();
    [GeneratedRegex(@"<script.*?</script>", RegexOptions.Singleline)]
    private static partial Regex ScriptHtmlPattern();
}
