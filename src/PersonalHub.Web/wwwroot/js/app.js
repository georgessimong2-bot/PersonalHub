window.downloadFile = function (bytes, fileName) {

    console.log("downloadFile called");

    const blob = new Blob(
        [new Uint8Array(bytes)],
        {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
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