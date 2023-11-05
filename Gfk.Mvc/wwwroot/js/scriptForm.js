var join = document.querySelector(".join");
var main = document.querySelectorAll(".main");
var cont = document.querySelectorAll(".cont");
var pay = document.querySelector(".pay");
var back = document.querySelectorAll(".back");
var pbar = document.querySelectorAll(".pbar li");
var progressba = document.querySelector(".pbar");
let formnumber = 0;

join.onclick = function ()
{
    if (!validateform())
    {
        return false;
    };
    formnumber++;
    updateform();
    progress_forward();
}

cont.forEach((contin) =>
{
    contin.onclick = function ()
    {
        if (!validateform())
        {
            return false;
        };
        formnumber++;
        updateform();
        progress_forward();
    }
});

pay.onclick = function ()
{
    if (!validateform())
    {
        return false;
    };
    formnumber++;
    updateform();
    var progressba = document.querySelector(".pbar");
    progressba.classList.add('d-none');
}

back.forEach((contin) =>
{
    contin.onclick = function ()
    {
        formnumber--;
        updateform();
        progress_back()
    }
});

function updateform()
{
    main.forEach((page) =>
    {
        page.classList.add('hidden');
        page.classList.remove('block');

    });
    main[formnumber].classList.remove('hidden');
    main[formnumber].classList.add('block');
}
function validateform()
{
    var validate = true;
    var all = document.querySelectorAll(".main.block input");
    all.forEach((e) =>
    {
        e.classList.remove('warning');
        if (e.hasAttribute('require'))
        {
            if (e.value.length == 0)
            {
                validate = false;
                e.classList.add('warning');
            }
        }
    });
    return validate;
}

function progress_forward()
{

    pbar[formnumber].classList.add('active');

}

function progress_back()
{
    var pi = formnumber + 1;
    pbar[pi].classList.remove('active');

}













document.addEventListener('DOMContentLoaded', () =>
{
    for (const el of document.querySelectorAll("[placeholder][data-slots]"))
    {
        const pattern = el.getAttribute("placeholder"),
            slots = new Set(el.dataset.slots || "_"),
            prev = (j => Array.from(pattern, (c, i) => slots.has(c) ? j = i + 1 : j))(0),
            first = [...pattern].findIndex(c => slots.has(c)),
            accept = new RegExp(el.dataset.accept || "\\d", "g"),
            clean = input =>
            {
                input = input.match(accept) || [];
                return Array.from(pattern, c =>
                    input[0] === c || slots.has(c) ? input.shift() || c : c
                );
            },
            format = () =>
            {
                const [i, j] = [el.selectionStart, el.selectionEnd].map(i =>
                {
                    i = clean(el.value.slice(0, i)).findIndex(c => slots.has(c));
                    return i < 0 ? prev[prev.length - 1] : back ? prev[i - 1] || first : i;
                }); el.value = clean(el.value).join``; el.setSelectionRange(i, j); back = false;
            }; let back = false; el.addEventListener("keydown", (e) => back = e.key === "Backspace");
        el.addEventListener("input", format);
        el.addEventListener("focus", format);
        el.addEventListener("blur", () => el.value === pattern && (el.value = ""));
    }
});