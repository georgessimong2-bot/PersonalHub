window.downloadFile = function (bytes, fileName, contentType) {

    const type = contentType || (fileName && fileName.toLowerCase().endsWith(".pdf")
        ? "application/pdf"
        : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

    const blob = new Blob(
        [new Uint8Array(bytes)],
        {
            type: type
        });

    const url = URL.createObjectURL(blob);

    const link = document.createElement("a");
    link.href = url;
    link.download = fileName;

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    URL.revokeObjectURL(url);
};