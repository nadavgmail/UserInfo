
function getAjax(url) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url,
            success: data => {
                resolve(data);
            },
            error: err => {
                reject(err);
            }
        })
    });
} 
//
function postAjax(url,dataOject) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url,
            type: "POST",
            data: dataOject,
            success: data => {
                resolve(data);
            },
            error: err => {
                reject(err);
            }
        })
    });
} 




$(function () {
    $(".getIpInfo").on("click",async function () {
        
        const ip = $(this).data('ip');
        const url = `/users/IpInfo?ipAdress=${ip}`;
        const data = await getAjax(url);
        $(".modal-body").html(data);
        $('#IPCardModelData').modal('show');
    });

    $('input[type=search]').on('search', function () {
        $("#userData tr").removeClass("hideElement");
    });

    $("input[type=search]").on("keyup", function () {
        const searchValue = $("input[type=search]").val().trim();
        if (searchValue === '') {
            $("#userData tr").removeClass("hideElement");
            return;
        }
        $("#userData tr").addClass("hideElement");
        const userNameTd = $(".username");
        let tdText = '';
        for (const element of userNameTd) {
            {
                tdText = $(element).text().trim();
                if (tdText.includes(searchValue)) {
                    $(element).closest("tr").removeClass("hideElement");
                }
            }
        }
    });






    $(".deleteUser").on("click", async function () {
        /*let geturl = `/users/DeleteConfirmedAajax/${this.id}`;*/
        const url = "/users/DeleteConfirmedAajax";
        if (confirm("delete ?")) {
            try {
               
                const data = await postAjax(url, { id: this.id });
                if (data.status === "fail") {
                    alert('cant delete user');
                    return;
                }
                $(this).closest("tr").remove();

            }
            catch (err) {
                alert(err.message)
            }
        }
    });
});