import ConfigStore from '../store/configStore';
import fetchSafe from '../services/fetchSafe';
import type { Configuration, TokenEndpointConfiguration } from '../models/configuration';

async function ensureTokenConfig(config:Configuration) {
  let error;
  const cfg = config;
  const endpoint = cfg.authentication.openIdConfigurationEndpoint;
  const response = await fetchSafe<TokenEndpointConfiguration>(endpoint);
  if (!response.error) {
    cfg.authentication.endpointConfiguration = response.data;
  } else {
    error = `The authorization config could not be loaded (${response.error})`;
  }
  return { config: cfg, error };
}

async function ensureConfig() {
  let error;
  const store = ConfigStore.create();
  let config = store.get();
  const response = await fetchSafe<Configuration>('/config.json');
  if (response.error) {
    error = `The config file could not be loaded (${response.error})`;
    return { config: null, error };
  }
  config = response.data;

  const { config: configIncToken, error: tokenError } = await ensureTokenConfig(config);
  if (tokenError) {
    return { config: null, error: tokenError };
  }
  config = configIncToken;

  store.set(configIncToken);
  return { config, error };
}

export const init = (() => ensureConfig());
