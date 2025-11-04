#define _XOPEN_SOURCE 700

#include <dirent.h>
#include <errno.h>
#include <fcntl.h>
#include <ftw.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/stat.h>
#include <sys/uio.h>
#include <unistd.h>

#define ERR(source)                                                \
  (perror(source), fprintf(stderr, "%s:%d\n", __FILE__, __LINE__), \
   exit(EXIT_FAILURE))

// join 2 path. returned pointer is for newly allocated memory and must be freed
char* join_paths(const char* path1, const char* path2) {
  char* res;
  const int l1 = strlen(path1);
  if (path1[l1 - 1] == '/') {
    res = malloc(strlen(path1) + strlen(path2) + 1);
    if (!res) ERR("malloc");
    strcpy(res, path1);
  } else {
    res =
        malloc(strlen(path1) + strlen(path2) + 2);  // additional space for "/"
    if (!res) ERR("malloc");
    strcpy(res, path1);
    res[l1] = '/';
    res[l1 + 1] = 0;
  }
  return strcat(res, path2);
}
static char* skip_spaces(char* s) {
  while (*s == ' ' || *s == '\t') s++;
  return s;
}

void usage(int argc, char** argv) {
  (void)argc;
  fprintf(stderr, "USAGE: %s path\n", argv[0]);
  exit(EXIT_FAILURE);
}
static void read_metadata_file(const char* path, char** author, char** title,
                               char** genre) {
  FILE* f = fopen(path, "r");
  if (!f) ERR("fopen");

  char* line = NULL;
  size_t line_size = 0;
  ssize_t nread;

  while ((nread = getline(&line, &line_size, f)) != -1) {
    // satır sonundaki '\n' varsa temizle
    if (nread > 0 && line[nread - 1] == '\n') line[nread - 1] = '\0';

    // anahtar ve değeri ayır
    char* colon = strchr(line, ':');
    if (!colon) continue;  // geçersiz satır

    *colon = '\0';
    char* key = line;
    char* value = skip_spaces(colon + 1);

    // hangi anahtar olduğunu kontrol et
    if (strcmp(key, "author") == 0) {
      free(*author);
      *author = strdup(value);
      if (!*author) ERR("strdup");
    } else if (strcmp(key, "title") == 0) {
      free(*title);
      *title = strdup(value);
      if (!*title) ERR("strdup");
    } else if (strcmp(key, "genre") == 0) {
      free(*genre);
      *genre = strdup(value);
      if (!*genre) ERR("strdup");
    }
    // diğer anahtarlar (latin_title, incipit vs.) bizi ilgilendirmiyor
  }

  free(line);
  if (fclose(f)) ERR("fclose");
}
int main(int argc, char** argv) {
  if (argc != 2) usage(argc, argv);
  char* author = NULL;
  char* title = NULL;
  char* genre = NULL;

  read_metadata_file(argv[1], &author, &title, &genre);

  if (author)
    printf("author: %s\n", author);
  else
    printf("author: missing!\n");

  if (title)
    printf("title: %s\n", title);
  else
    printf("title: missing!\n");

  if (genre)
    printf("genre: %s\n", genre);
  else
    printf("genre: missing!\n");

  free(author);
  free(title);
  free(genre);

  return EXIT_SUCCESS;
}
