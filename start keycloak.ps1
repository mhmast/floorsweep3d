docker run -e KEYCLOAK_USER=admin -e KEYCLOAK_PASSWORD=admin -p 8080:8080 -p 8443:8443 --name keycloak -v "C:\Ontwik\Floorsweep\Keycloak\Data:/opt/jboss/keycloak/standalone/data" -v "C:\Ontwik\Floorsweep\Keycloak\Cert:/etc/x509/https" jboss/keycloak