document.addEventListener("DOMContentLoaded", () => {
    document.addEventListener("click", (event) => {
        const link = event.target.closest(".page-link");
        if (!link) return; // Если клик был не по ссылке пагинатора, ничего не делаем

        event.preventDefault();


        const url = link.href;

        //$("#fruit-list-container").load(url);
        

        fetch(url, {
            headers: { "X-Requested-With": "XMLHttpRequest" },
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Ошибка при загрузке данных.");
                }
                return response.text();
            })
            .then((html) => {
                const container = document.querySelector("#fruit-list-container");
                if (container) {
                    container.innerHTML = html; // Заменяем содержимое списка фруктов
                } else {
                    console.error("Контейнер #fruit-list-container не найден.");
                }
            })
            .catch((error) => console.error("Ошибка:", error));
    });
});
