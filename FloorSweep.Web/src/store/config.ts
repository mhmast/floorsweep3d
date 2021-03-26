import type { Configuration } from "../models/configuration";


export function get(){ 
return ((typeof window !== "undefined")?JSON.parse(localStorage.getItem("config"))
: null ) as Configuration;
};

export function set(conf:Configuration){
    if(typeof window !== "undefined")
    {
        localStorage.setItem("config",JSON.stringify(conf));
    }
}
