import {get as getConfig,set as setConfig} from "../store/config"
import fetchSafe from "../services/fetchSafe"
import type { Configuration,TokenEndpointConfiguration } from "../models/configuration";


export const init = (async () => await ensureConfig()  )();

async function ensureConfig() {
  let config = getConfig();
  let error;
  if (!config) {
      const response = await fetchSafe<Configuration>("/config.json");
      if (response.error) {
        error = `The config file could not be loaded (${response.error})`;
        return {config,error};
      }  
      config = response.data;
  }
  const {config:configIncToken,error:tokenError} = await ensureTokenConfig(config)
  if(tokenError)
  {
    return {config:null,error:tokenError};
  }
  config = configIncToken;
  setConfig(configIncToken);
  
  return {config,error};
}

async function ensureTokenConfig(config:Configuration) {
  let error;
  if (!config.authentication.endpointConfiguration) {
      const response = await fetchSafe<TokenEndpointConfiguration>(config.authentication.openIdConfigurationEndpoint);
      if (!response.error) {
        config.authentication.endpointConfiguration = response.data
      } else {
        error = `The authorization config could not be loaded (${response.error})`;
      }
  }
  return {config,error};
}