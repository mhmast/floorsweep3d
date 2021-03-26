

export function get(){ 
return ((typeof window !== "undefined")?JSON.parse(localStorage.getItem("token"))
: null ) as string;
};

export function set(token:string){
    if(typeof window !== "undefined")
    {
        localStorage.setItem("token",token);
    }
}
