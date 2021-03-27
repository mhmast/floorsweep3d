import {loginRedirect} from "../services/authenticationService";

export default async function fetchSafe<T>(input:RequestInfo,init?:RequestInit): Promise<Result<T>>{
    try {
        const response = await fetch(input,init);
        if (response.ok) {
          const data = await response.json() as T;
          return {data,statusCode:response.status,error:null};  
        }
        else if(response.status == 401 || response.status == 403){
            loginRedirect();           
        } 
        else{
            return {data:null,statusCode:response.status,error:response.statusText};         
        }
      } catch (e) {
        return {data:null,statusCode:500,error:e};
      }
};

export interface Result<T>
{
    data:T,
    error:string,
    statusCode:number
};