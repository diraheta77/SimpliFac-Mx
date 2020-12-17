using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Text;
using Microsoft.AspNetCore.Html;


namespace simplifile.Helpers
{
    public static class HtmlHelpers
    {
        public static void AddCssBody(this IHtmlHelper html, string css)
        {
            html.ViewData["CssBody"] = css;
        }

        public static string GetCssBody(this IHtmlHelper html)
        {
            return html.ViewData.ContainsKey("CssBody") ? html.ViewData["CssBody"].ToString() : "";
        }

        public static HtmlString Notifications(this IHtmlHelper html)
        {
            StringBuilder content = new StringBuilder();
            
            var baseUrl = html.ViewContext.HttpContext.Request.PathBase.Value;

            content.AppendLine($"<script src='{baseUrl}/lib/notify/notify.min.js'></script>");

            content.AppendLine(" <script type='text/javascript'>");
            content.AppendLine(@"var options = {
                        // whether to hide the notification on click
                        clickToHide: true,
                        // whether to auto-hide the notification
                        autoHide: true,
                        // if autoHide, hide after milliseconds
                        autoHideDelay: 5000,
                        // show the arrow pointing at the element
                        arrowShow: true,
                        // arrow size in pixels
                        arrowSize: 5,
                        // position defines the notification position though uses the defaults below
                        position: '...',
                        // default positions
                        elementPosition: 'bottom left',
                        globalPosition: 'top right',
                        // default style
                        style: 'bootstrap',
                        // default class (string or [string])
                        className: 'error',
                        // show animation
                        showAnimation: 'slideDown',
                        // show animation duration
                        showDuration: 400,
                        // hide animation
                        hideAnimation: 'slideUp',
                        // hide animation duration
                        hideDuration: 200,
                        // padding between element and notification
                        gap: 2
                    };
                    var msgs = '';
            ");

            if (html.TempData.ContainsKey("Error"))
            {
                content.AppendLine($"$.notify('{html.TempData["Error"]}','error');");
            }
            if (html.TempData.ContainsKey("Success"))
            {
                content.AppendLine($"$.notify('{html.TempData["Success"]}','success');");
            }

            content.AppendLine("</script>");

            return new HtmlString(content.ToString());

        }

        public static HtmlString Modal(this IHtmlHelper html, string btnText, string btnIcon, string btnClass, string dataUrl, string modalTitle, string modalIcon, string btnSubmitText, string size)
        {
            if (string.IsNullOrEmpty(btnClass)) btnClass = "btn btn-primary";
            if (string.IsNullOrEmpty(btnSubmitText)) btnSubmitText = "Guardar";
            if (string.IsNullOrEmpty(size)) btnSubmitText = "md";
            var modalId = Guid.NewGuid().ToString("N");
            StringBuilder content = new StringBuilder();




            content.Append($@"
                <div class='modal fade' id='{modalId}'>
                  <div class='modal-dialog modal-{size}'>
                    <div class='modal-content'>
                      <div class='modal-header'>
                        <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                          <span aria-hidden='true'>&times;</span></button>
                        <h4 class='modal-title'>
                            <i class='{modalIcon}'></i>
                            {modalTitle}
                        </h4>
                      </div>
                      <div class='modal-body'>
                        <p>One fine body&hellip;</p>
                      </div>
                      <div class='modal-footer'>
                        <button type='button' class='btn btn-default pull-left' data-dismiss='modal'>
                            <i class='fa  fa-times'></i>
                            Cerrar
                        </button>
                        <button type='button' class='btn btn-primary' data-save='modal'>
                             <i class='fa fa-check-square-o'></i>
                             {btnSubmitText}
                         </button>
                      </div>
                    </div>
                    <!-- /.modal-content -->
                  </div>
                </div>");
            content.AppendLine($"<button id='btn-{modalId}' type='button' class='{btnClass}' data-toggle='modal' data-target='#add-contact' data-url='{dataUrl}'>");
            content.AppendLine($"<i class='{btnIcon}'></i> {btnText}");
            content.AppendLine("</button>");

            content.AppendLine(" <script type='text/javascript'>");

            content.AppendLine(" $(function () {");
            content.Append(@"
                $('#btn-{0}').click(function (event) {
                    var _th = $(this);
                    var url = _th.data('url');
            
                    $.get(url).done(function (data) {
                        $('#{0} .modal-body').html(data);
                        $('#{0}').modal('show');
                    });
                });
            ".Replace("{0}", modalId));

            content.Append(@"
                $('#{0}').on('click', '[data-save=\'modal\']', function (event) {
                    event.preventDefault();

                    var form = $(this).parents('.modal').find('form');
                    var actionUrl = form.attr('action');
                    var dataToSend = form.serialize();
                    

                    $.post(actionUrl, dataToSend).done(function (data) {
                        
                        if(data.redirect == true)
                        {
                            window.location = data.url;
                            return;
                        }
                        //var newBody = $('.modal-body', data);
                        $('#{0}').find('.modal-body').html(data);

                        var isValid = $('#{0} .modal-body').find('[name=\'IsValid\']').val() == 'True';
                        if (isValid)
                        {
                                $('#{0}').modal('hide');
                        }
                    });
                });".Replace("{0}", modalId));

            content.AppendLine("});");
            content.AppendLine("</script>");

            return new HtmlString(content.ToString());
        }

        public static HtmlString Modal(this IHtmlHelper html, string modalId, string btnSelector, string modalTitle, string modalIcon, string btnSubmitText, string size,bool onSubmit = true)
        {
            if (string.IsNullOrEmpty(btnSubmitText)) btnSubmitText = "Guardar";
            if (string.IsNullOrEmpty(size)) btnSubmitText = "md";

            StringBuilder content = new StringBuilder();

            content.Append($@"
                <div class='modal fade' id='{modalId}'>
                  <div class='modal-dialog modal-{size}'>
                    <div class='modal-content'>
                      <div class='modal-header'>
                        <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                          <span aria-hidden='true'>&times;</span></button>
                        <h4 class='modal-title'>
                            <i class='{modalIcon}'></i>
                            {modalTitle}
                        </h4>
                      </div>
                      <div class='modal-body'>
                        <p>One fine body&hellip;</p>
                      </div>
                      <div class='modal-footer'>
                        <button type='button' class='btn btn-default pull-left' data-dismiss='modal'>
                            <i class='fa  fa-times'></i>
                            Cerrar
                        </button>
                        <button type='button' class='btn btn-primary' data-save='modal'>
                             <i class='fa fa-check-square-o'></i>
                             {btnSubmitText}
                         </button>
                      </div>
                    </div>
                    <!-- /.modal-content -->
                  </div>
                </div>");
            content.AppendLine(" <script type='text/javascript'>");

            content.AppendLine(" $(function () {");
            content.Append(@"
                $(document).on('click','{0}',function (event) {
                    var _th = $(this);
                    var url = _th.data('url');
            
                    $.get(url).done(function (data) {
                        $('#{1} .modal-body').html(data);
                        $('#{1}').modal('show');
                    });
                });
            ".Replace("{0}", btnSelector).Replace("{1}", modalId));

            if(onSubmit)
            {
                content.Append(@"
                $('#{0}').on('click', '[data-save=\'modal\']', function (event) {
                    event.preventDefault();

                    var form = $(this).parents('.modal').find('form');
                    var actionUrl = form.attr('action');
                    var dataToSend = form.serialize();

                    $.post(actionUrl, dataToSend).done(function (data) {
                        
                        if(data.redirect == true)
                        {
                            window.location = data.url;
                            return;
                        }
                        //var newBody = $('.modal-body', data);
                        $('#{0}').find('.modal-body').html(data);

                        var isValid = $('#{0} .modal-body').find('[name=\'IsValid\']').val() == 'True';
                        if (isValid)
                        {
                                $('#{0}').modal('hide');
                        }
                    });
                });".Replace("{0}", modalId));
            }
            

            content.AppendLine("});");
            content.AppendLine("</script>");

            return new HtmlString(content.ToString());
        }

        public static IHtmlContent RawViewData(this IHtmlHelper html, string key)
        {
            if (html.ViewData.ContainsKey(key))
            {
                return html.Raw(html.ViewData[key]);
            }
            return html.Raw("");
        }

    }
}
