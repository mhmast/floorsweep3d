import type { TokenData } from "../models/tokenData";


export function get(){ 
const data = {token:null,challenge:null}  as TokenData;
return ((typeof window !== "undefined")?JSON.parse(localStorage.getItem("tokenData"))??data
: data);
};

export function set(tokenData:TokenData){
    if(typeof window !== "undefined")
    {
        localStorage.setItem("tokenData",JSON.stringify(tokenData));
    }
}
