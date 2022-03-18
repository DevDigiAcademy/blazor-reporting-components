// Interop file to render the Bold Reports component with properties.
window.BoldReports = {
    RenderViewer: function (elementID, reportViewerOptions) {
        $("#" + elementID).boldReportViewer({
            reportPath: reportViewerOptions.reportName,
            reportServiceUrl: reportViewerOptions.serviceURL
        });
    },
    RenderDesigner: function (elementID, reportDesignerOptions) {
        $("#" + elementID).boldReportDesigner({
            serviceUrl: reportDesignerOptions.serviceURL
        });
    }
}

window.DisposeReportsObject = () => {
    var reportViewerElement = document.querySelector('.e-reportviewer.e-js');
    if (reportViewerElement) {
        $(reportViewerElement).data('boldReportViewer').destroy(); //Destroy the report viewer processing objects.
    }
    return true;
}